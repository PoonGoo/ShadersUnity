using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RGBSplitFeature : ScriptableRendererFeature
{
    private RGBSplitPass rgbSplitPass;

    public override void Create()
    {
        rgbSplitPass = new RGBSplitPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var stack = VolumeManager.instance.stack;
        var rgbSplitVolume = stack.GetComponent<RGBSplitVolume>();
        if (rgbSplitVolume == null || !rgbSplitVolume.IsActive())
            return;

        renderer.EnqueuePass(rgbSplitPass);
    }
}
