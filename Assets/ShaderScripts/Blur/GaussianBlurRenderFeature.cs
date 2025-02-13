using UnityEngine.Rendering.Universal;

public class GaussianBlurRenderFeature : ScriptableRendererFeature
{
    private GaussianBlurPass gaussianBlurPass;

    public override void Create()
    {
        gaussianBlurPass = new GaussianBlurPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(gaussianBlurPass); 
    }

}
