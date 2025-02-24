using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenu("Custom/RGBSplit")]
public class RGBSplitVolume : VolumeComponent, IPostProcessComponent
{
    public BoolParameter isEnabled = new BoolParameter(false);

    public ClampedFloatParameter rgbSplitAmount = new ClampedFloatParameter(0.002f, 0.0f, 0.01f);

    public ClampedFloatParameter pulseFrequency = new ClampedFloatParameter(2.0f, 0.0f, 10.0f);

    public ClampedFloatParameter pulseAmplitude = new ClampedFloatParameter(0.5f, 0.0f, 1.0f);

    public bool IsActive() => isEnabled.value;
    public bool IsTileCompatible() => false;
}
