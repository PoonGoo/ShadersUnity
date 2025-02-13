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
        // Set the pass event as needed (e.g., before post-processing)
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
        // Check that post-processing is enabled
        if (!renderingData.cameraData.postProcessEnabled)
            return;

        // Get the active volume component
        var stack = VolumeManager.instance.stack;
        var rgbSplitVolume = stack.GetComponent<RGBSplitVolume>();
        if (rgbSplitVolume == null || !rgbSplitVolume.IsActive())
            return;

        // Set the shader parameter from the volume
        material.SetFloat("_RGBSplitAmount", rgbSplitVolume.rgbSplitAmount.value);

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
