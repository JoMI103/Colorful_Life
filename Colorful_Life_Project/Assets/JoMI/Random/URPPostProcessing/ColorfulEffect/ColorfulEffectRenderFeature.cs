using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;

public class ColorfulEffectRenderFeature : ScriptableRendererFeature
{
    private ColorfulPass _colorfulPass;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
            renderer.EnqueuePass(_colorfulPass);
    }

    public override void Create()
    {
        _colorfulPass = new ColorfulPass();

    }

    [System.Serializable]
    class ColorfulPass : ScriptableRenderPass
    {
        private Material _material;
        //int _tintId = Shader.PropertyToID("_Temp");
        RTHandle _source, _destination;

        public ColorfulPass()
        {

            //if (!_material) _material = CoreUtils.CreateEngineMaterial("CustomPost/ScreenTint");
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            Shader colorfulShader = Shader.Find("CustomPost/ColorfullEffect");
            _material = new Material(colorfulShader);

            _source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            // RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            // cmd.GetTemporaryRT(_tintId, desc, FilterMode.Bilinear);
            // _tint = new(_tintId);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            //CommandBuffer cmd = CommandBufferPool.Get();
            CommandBuffer cmd = CommandBufferPool.Get("okok");
            cmd.Clear();

            VolumeStack volumes = VolumeManager.instance.stack;
            CustomPostColurfulEffect colorfulData = volumes.GetComponent<CustomPostColurfulEffect>();

            _destination = _source;

            if (colorfulData.IsActive())
            {
                
                _material.SetFloat(Shader.PropertyToID("_intensity"), colorfulData.Intensity.value);


                switch (colorfulData.mode.value)
                {
                    case CustomPostColurfulEffect.clEffectMode.BlackWhite:
                        {
                            _material.EnableKeyword("BlackWhite");
                            break;
                        }
                    case CustomPostColurfulEffect.clEffectMode.Rage:
                        {
                            _material.EnableKeyword("Rage");
                            break;
                        }
                    case CustomPostColurfulEffect.clEffectMode.verde:
                        {
                            _material.EnableKeyword("verde");
                            break;
                        }
                    case CustomPostColurfulEffect.clEffectMode.Void:
                        {
                            _material.EnableKeyword("Void");
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
