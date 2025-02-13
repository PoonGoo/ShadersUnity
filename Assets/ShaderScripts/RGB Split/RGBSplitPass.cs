using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RGBSplitPass : ScriptableRenderPass
{
    private Material material;
    private RenderTargetIdentifier source;
    private int temporaryRTId = Shader.PropertyToID("_RGBSplitTempRT");

    public RGBSplitPass()
    {
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        material = CoreUtils.CreateEngineMaterial("Hidden/RGBSplit");
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        source = renderingData.cameraData.renderer.cameraColorTarget;
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        cmd.GetTemporaryRT(temporaryRTId, descriptor, FilterMode.Bilinear);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (!renderingData.cameraData.postProcessEnabled)
            return;

        var stack = VolumeManager.instance.stack;
        var rgbSplitVolume = stack.GetComponent<RGBSplitVolume>();
        if (rgbSplitVolume == null || !rgbSplitVolume.IsActive())
            return;

        material.SetFloat("_RGBSplitAmount", rgbSplitVolume.rgbSplitAmount.value);
        material.SetFloat("_PulseFrequency", rgbSplitVolume.pulseFrequency.value);
        material.SetFloat("_PulseAmplitude", rgbSplitVolume.pulseAmplitude.value);

        CommandBuffer cmd = CommandBufferPool.Get("RGB Split Pass");
        cmd.Blit(source, temporaryRTId, material);
        cmd.Blit(temporaryRTId, source);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if (cmd == null)
            throw new System.ArgumentNullException("cmd");
        cmd.ReleaseTemporaryRT(temporaryRTId);
    }
}
