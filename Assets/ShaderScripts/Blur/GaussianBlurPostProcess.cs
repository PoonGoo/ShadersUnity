using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/Gaussian Blur", typeof(UniversalRenderPipeline))]

public class GaussianBlurPostProcess : VolumeComponent, IPostProcessComponent
{
    [Tooltip("Standard deviation of the gaussian blur")]
    public FloatParameter blurIntensity = new ClampedFloatParameter(0.0f, 0.0f, 50.0f);

    public bool IsActive()
    {
        return (blurIntensity.value > 0.0f) && active;
    }

    public bool IsTileCompatible() => true;
}