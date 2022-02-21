using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScreenFadeFeature : ScriptableRendererFeature
{
    public FadeSettings settings = null;
    private ScreenFadePass renderPass = null;

    public override void Create()
    {
        renderPass = new ScreenFadePass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.AreValid())
        {
            renderer.EnqueuePass(renderPass);
        }
    }
}

[System.Serializable]
public class FadeSettings
{
    public bool isEnabled = true;
    public string profilerTag = "ScreenFade";

    public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    public Material material = null;

    [System.NonSerialized] public Material runtimeMaterial = null;

    public bool AreValid()
    {
        return (runtimeMaterial != null) && isEnabled;
    }
}
