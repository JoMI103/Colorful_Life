using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Runtime.CompilerServices;

namespace UnityEngine.Rendering.Universal
{
    [System.Serializable]
    public class CustomPostProcessPass : ScriptableRenderPass
    {
        RenderTextureDescriptor GetCompatibleDescriptor()
                => GetCompatibleDescriptor(m_Descriptor.width, m_Descriptor.height, m_Descriptor.graphicsFormat);

        RenderTextureDescriptor GetCompatibleDescriptor(int width, int height, GraphicsFormat format, DepthBits depthBufferBits = DepthBits.None)
            => GetCompatibleDescriptor(m_Descriptor, width, height, format, depthBufferBits);

        internal static RenderTextureDescriptor GetCompatibleDescriptor(RenderTextureDescriptor desc, int width, int height, GraphicsFormat format, DepthBits depthBufferBits = DepthBits.None)
        {
            desc.depthBufferBits = (int)depthBufferBits;
            desc.msaaSamples = 1;
            desc.width = width;
            desc.height = height;
            desc.graphicsFormat = format;
            return desc;
        }


        private Material m_bloomMaterial;
        private Material m_compositeMaterial;
        GraphicsFormat m_GraphicsFormat;
        BenDayBloomEffectComponent m_BloomEffect;

        RenderTextureDescriptor m_Descriptor;

        const int k_MaxPyramidSize = 16;
        private int[] _BloomMipUp;
        private int[] _BloomMipDown;
        private RTHandle[] m_BloomMipUp;
        private RTHandle[] m_BloomMipDown;
        private GraphicsFormat hdrFormat;


     
        public CustomPostProcessPass(Material bloomMaterial, Material compositeMaterial)
        {


            m_bloomMaterial = bloomMaterial;
            m_compositeMaterial = compositeMaterial;

            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

            _BloomMipUp = new int[k_MaxPyramidSize];
            _BloomMipDown = new int[k_MaxPyramidSize];
            m_BloomMipUp = new RTHandle[k_MaxPyramidSize];
            m_BloomMipDown = new RTHandle[k_MaxPyramidSize];

            for (int i = 0; i < k_MaxPyramidSize; i++)
            {
                _BloomMipUp[i] = Shader.PropertyToID("_BloomMipUp" + i);
                _BloomMipDown[i] = Shader.PropertyToID("_BloomMipDown" + i);

                m_BloomMipUp[i] = RTHandles.Alloc(_BloomMipUp[i], name: "_BloomMipUp" + i);
                m_BloomMipDown[i] = RTHandles.Alloc(_BloomMipDown[i], name: "_BloomMipDown" + i);
            }

            const FormatUsage usage = FormatUsage.Linear | FormatUsage.Render;
            if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, usage))
            {
                hdrFormat = GraphicsFormat.B10G11R11_UFloatPack32;
            }
            else
            {
                hdrFormat = QualitySettings.activeColorSpace == ColorSpace.Linear
                    ? GraphicsFormat.R8G8B8A8_SRGB
                    : GraphicsFormat.R8G8B8A8_UNorm;
            }


        }


        void SetupBloom(CommandBuffer cmd, RTHandle source)
        {
            // Start at half-res
            int downres = 1;
            int tw = m_Descriptor.width >> downres;
            int th = m_Descriptor.height >> downres;

            // Determine the iteration count
            int maxSize = Mathf.Max(tw, th);
            int iterations = Mathf.FloorToInt(Mathf.Log(maxSize, 2f) - 1);
            int mipCount = Mathf.Clamp(iterations, 1, m_BloomEffect.maxIterations.value);

            // Pre-filtering parameters
            float clamp = m_BloomEffect.clamp.value;
            float threshold = Mathf.GammaToLinearSpace(m_BloomEffect.threshold.value);
            float thresholdKnee = threshold * 0.5f; // Hardcoded soft knee

            // Material setup
            float scatter = Mathf.Lerp(0.05f, 0.95f, m_BloomEffect.scater.value);
            var bloomMaterial = m_bloomMaterial;

            bloomMaterial.SetVector("_Params", new Vector4(scatter, clamp, threshold, thresholdKnee));

            // Prefilter
            var desc = GetCompatibleDescriptor(tw, th, hdrFormat);
            for (int i = 0; i < mipCount; i++)
            {
                RenderingUtils.ReAllocateIfNeeded(ref m_BloomMipUp[i], desc, FilterMode.Bilinear, TextureWrapMode.Clamp, name: m_BloomMipUp[i].name);
                RenderingUtils.ReAllocateIfNeeded(ref m_BloomMipDown[i], desc, FilterMode.Bilinear, TextureWrapMode.Clamp, name: m_BloomMipDown[i].name);
                desc.width = Mathf.Max(1, desc.width >> 1);
                desc.height = Mathf.Max(1, desc.height >> 1);
            }

            Blitter.BlitCameraTexture(cmd, source, m_BloomMipDown[0], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 0);

            // Downsample - gaussian pyramid
            var lastDown = m_BloomMipDown[0];
            for (int i = 1; i < mipCount; i++)
            {
                // Classic two pass gaussian blur - use mipUp as a temporary target
                //   First pass does 2x downsampling + 9-tap gaussian
                //   Second pass does 9-tap gaussian using a 5-tap filter + bilinear filtering
                Blitter.BlitCameraTexture(cmd, lastDown, m_BloomMipUp[i], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 1);
                Blitter.BlitCameraTexture(cmd, m_BloomMipUp[i], m_BloomMipDown[i], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 2);

                lastDown = m_BloomMipDown[i];
            }

            // Upsample (bilinear by default, HQ filtering does bicubic instead
            for (int i = mipCount - 2; i >= 0; i--)
            {
                var lowMip = (i == mipCount - 2) ? m_BloomMipDown[i + 1] : m_BloomMipUp[i + 1];
                var highMip = m_BloomMipDown[i];
                var dst = m_BloomMipUp[i];
                mip0 = lowMip;
                //cmd.SetGlobalTexture("_SourceTexLowMip", lowMip);
                Blitter.BlitCameraTexture(cmd, highMip, dst, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 3);
            }


            m_compositeMaterial.SetTexture("_Bloom_Texture", m_BloomMipUp[0]);
            //cmd.SetGlobalTexture("_Bloom_Texture", m_BloomMipUp[0]);
            cmd.SetGlobalFloat("_BloomIntensity", m_BloomEffect.intensity.value);

        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            m_Descriptor = renderingData.cameraData.cameraTargetDescriptor;
        }

         RTHandle m_CameraColorTarget, m_CameraDepthTarget;
        private RTHandle mip0;

        public void SetTarget(RTHandle cameraColorTargetHandle, RTHandle cameraDepthTargetHandle)
        {
            
            m_CameraColorTarget = cameraColorTargetHandle;
            m_CameraDepthTarget = cameraDepthTargetHandle;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            VolumeStack stack = VolumeManager.instance.stack;
            m_BloomEffect = stack.GetComponent<BenDayBloomEffectComponent>();
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler("Benday bloom effect")))
            {
                SetupBloom(cmd, m_CameraColorTarget);
                m_compositeMaterial.SetFloat("_Cutoff", m_BloomEffect.dotsCutOff.value);
                m_compositeMaterial.SetFloat("_Density", m_BloomEffect.dotsDensity.value);
                m_compositeMaterial.SetVector("_Direction", m_BloomEffect.scrollDirection.value);
                m_compositeMaterial.SetFloat("_BloomIntensity", m_BloomEffect.intensity.value);
                m_compositeMaterial.SetTexture("_Bloom_Texture", mip0);
                Blitter.BlitCameraTexture(cmd, m_CameraColorTarget, m_CameraColorTarget, m_compositeMaterial, 0);
            }
            //Blitter.BlitCameraTexture(cmd, colorTexture, colorTexture, m_compositeMaterial, 0);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }


    }
}