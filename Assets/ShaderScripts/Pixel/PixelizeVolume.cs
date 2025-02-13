using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable, VolumeComponentMenu("Custom/Pixelize")]
public class PixelizeVolume : VolumeComponent, IPostProcessComponent
{
    public ClampedIntParameter screenHeight = new ClampedIntParameter(144, 32, 1080);
    public BoolParameter isEnabled = new BoolParameter(false);

    public bool IsActive() => isEnabled.value; // Ensure effect is only active when enabled
    public bool IsTileCompatible() => false; // Not a tile-based effect
}
