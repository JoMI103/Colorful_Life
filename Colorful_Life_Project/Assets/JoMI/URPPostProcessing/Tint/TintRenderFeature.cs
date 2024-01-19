using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;

public class TintRenderFeature : ScriptableRendererFeature
{
    private TintPass _tintPass;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
            renderer.EnqueuePass(_tintPass);
    }

    public override void Create()
    {
        _tintPass = new TintPass();

    }

    [System.Serializable]
    class TintPass : ScriptableRenderPass {
        private Material _material;
        //int _tintId = Shader.PropertyToID("_Temp");
        RTHandle _source ,_destination;

        public TintPass() {

            //if (!_material) _material = CoreUtils.CreateEngineMaterial("CustomPost/ScreenTint");
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            Shader tintShader = Shader.Find("CustomPost/ScreenTint");
            _material = new Material(tintShader);

            _source = renderingData.cameraData.renderer.cameraColorTargetHandle;

           // RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
           // cmd.GetTemporaryRT(_tintId, desc, FilterMode.Bilinear);
           // _tint = new(_tintId);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            //CommandBuffer cmd = CommandBufferPool.Get();
            CommandBuffer cmd = CommandBufferPool.Get("------------------");
            cmd.Clear();

            VolumeStack volumes = VolumeManager.instance.stack;
            CustomPostScreenTint tintData = volumes.GetComponent<CustomPostScreenTint>();

            _destination = _source;

            if (tintData.IsActive())
            {
                _material.SetColor(Shader.PropertyToID("_overlayColor"), tintData.tintColor.value);
                _material.SetFloat(Shader.PropertyToID("_intensity"), tintData.tintIntensity.value);


                switch (tintData.mode.value)
                {
                    case CustomPostScreenTint.TintMode.Multiply:
                        {
                            _material.EnableKeyword("MULTIPLY");
                            break;
                        }
                    case CustomPostScreenTint.TintMode.ColorBurn:
                        {
                            _material.EnableKeyword("COLORBURN");
                            break;
                        }
                    case CustomPostScreenTint.TintMode.LinearBurn:
                        {
                            _material.EnableKeyword("LINEARBURN");
                            break;
                        }
                    case CustomPostScreenTint.TintMode.Screen:
                        {
                            _material.EnableKeyword("SCREEN");
                            break;
                        }
                    case CustomPostScreenTint.TintMode.ColorDodge:
                        {
                            _material.EnableKeyword("COLORDODGE");
                            break;
                        }
                    case CustomPostScreenTint.TintMode.LinearDodge:
                        {
                            _material.EnableKeyword("LINEARDODGE");
                            break;
                        }
                }

                cmd.Blit(_destination, _source, _material, 0);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        


        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            
            //cmd.ReleaseTemporaryRT(_tintId);
        }
        
        
    }

}



