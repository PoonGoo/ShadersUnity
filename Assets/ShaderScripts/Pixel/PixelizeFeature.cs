using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelizeFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class CustomPassSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public int screenHeight = 144;
    }

    [SerializeField] private CustomPassSettings settings;
    private PixelizePass customPass;

    public override void Create()
    {
        customPass = new PixelizePass(settings);
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        VolumeStack stack = VolumeManager.instance.stack;
        PixelizeVolume pixelizeVolume = stack.GetComponent<PixelizeVolume>();

        if (pixelizeVolume == null || !pixelizeVolume.IsActive()) return;

        renderer.EnqueuePass(customPass);
    }

}


