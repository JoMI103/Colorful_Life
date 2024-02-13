using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class MultiPassRendererFeature : ScriptableRendererFeature
{
    class MultiPassPass : ScriptableRenderPass
    {
        private List<ShaderTagId> _tags;

        public MultiPassPass(List<string> tags)
        {
            _tags = new();

            foreach (string tag in tags)
            {
                _tags.Add(new ShaderTagId(tag));
            }

            this.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        }


        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }

        
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            FilteringSettings filteringSettings = FilteringSettings.defaultValue;

            foreach (ShaderTagId pass in _tags)
            {
                DrawingSettings drawingSettings = CreateDrawingSettings(pass, ref renderingData, SortingCriteria.CommonOpaque);
                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);
            }

            context.Submit();
        }


        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    public List<string> lightModePasses;
    private MultiPassPass mainPass;


    public override void Create()
    {
        mainPass = new(lightModePasses);
    }

 
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(mainPass);
    }
}


