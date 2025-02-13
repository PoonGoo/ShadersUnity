using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenu("Custom/Pixelize")]
public class PixelizeVolume : VolumeComponent, IPostProcessComponent
{
    public ClampedIntParameter screenHeight = new ClampedIntParameter(144, 32, 1080);
    public BoolParameter isEnabled = new BoolParameter(false);

    // Wave distortion 
    public ClampedFloatParameter waveFrequency = new ClampedFloatParameter(20.0f, 0.0f, 50.0f);
    public ClampedFloatParameter waveSpeed = new ClampedFloatParameter(3.0f, 0.0f, 10.0f);
    public ClampedFloatParameter waveAmplitude = new ClampedFloatParameter(0.005f, 0.0f, 0.02f);

    // Digital Glitch 
    public BoolParameter enableGlitch = new BoolParameter(false);
    public ClampedFloatParameter glitchIntensity = new ClampedFloatParameter(0.05f, 0.0f, 0.1f);

    public ClampedFloatParameter glitchFrequency = new ClampedFloatParameter(0.05f, 0.0f, 1.0f);

    // RGB Split properties
/*    public BoolParameter enableRGBSplit = new BoolParameter(false);
    public ClampedFloatParameter rgbSplitAmount = new ClampedFloatParameter(0.002f, 0.0f, 0.01f);*/

    public bool IsActive() => isEnabled.value;
    public bool IsTileCompatible() => false;
}
