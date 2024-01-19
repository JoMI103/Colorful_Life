using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Tutorial/CustomPostScreenTint", typeof(UniversalRenderPipeline))]
public class CustomPostScreenTint : VolumeComponent, IPostProcessComponent
{
    

    public TintParameter mode = new TintParameter(TintMode.Multiply);

    public FloatParameter tintIntensity = new(1);
    public ColorParameter tintColor = new(Color.white);

    public bool IsActive() => tintIntensity.value > 0;

    public bool IsTileCompatible() => true;

    public enum TintMode
    {
        Multiply,
        ColorBurn,
        LinearBurn,
        Screen,
        ColorDodge,
        LinearDodge,
    }

    [Serializable]
    public sealed class TintParameter : VolumeParameter<TintMode>
    {
        public TintParameter(TintMode value, bool overrideState = false) : base(value, overrideState) { }
    }
}
