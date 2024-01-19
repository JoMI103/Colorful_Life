using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


[Serializable, VolumeComponentMenuForRenderPipeline("Colerful/Effect", typeof(UniversalRenderPipeline))]
public class CustomPostColurfulEffect : VolumeComponent, IPostProcessComponent
{


    public clEffectParameter mode = new clEffectParameter(clEffectMode.BlackWhite);

    public FloatParameter Intensity = new(1);
    

    public bool IsActive() => Intensity.value > 0;

    public bool IsTileCompatible() => true;

    public enum clEffectMode
    {
        BlackWhite,
        Rage,
        verde,
        Void,
    }

    [Serializable]
    public sealed class clEffectParameter : VolumeParameter<clEffectMode>
    {
        public clEffectParameter(clEffectMode value, bool overrideState = false) : base(value, overrideState) { }
    }
}
