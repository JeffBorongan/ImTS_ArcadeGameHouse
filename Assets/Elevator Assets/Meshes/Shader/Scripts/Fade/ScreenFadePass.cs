using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenFadePass : ScriptableRenderPass
{
    private FadeSettings setting = null;

    public ScreenFadePass(FadeSettings newSettings)
    {
        setting = newSettings;
        renderPassEvent = newSettings.renderPassEvent;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer command = CommandBufferPool.Get(setting.profilerTag);

        RenderTargetIdentifier source = BuiltinRenderTextureType.CameraTarget;
        RenderTargetIdentifier destination = BuiltinRenderTextureType.CurrentActive;

        command.Blit(source, destination, setting.runtimeMaterial);
        context.ExecuteCommandBuffer(command);

        CommandBufferPool.Release(command);
    }
}
