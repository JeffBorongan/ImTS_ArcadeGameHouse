using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class ScreenFadeManager : MonoBehaviour
{
    public static ScreenFadeManager Instance { private set; get; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public ForwardRendererData rendererData = null;

    [Range(0, 1)] public float alpha = 1.0f;
    [Range(0, 5)] public float duration = 0.5f;

    private Material fadeMaterial = null;

    private void Start()
    {
        SetupFadeFeature();
    }

    private void SetupFadeFeature()
    {
        ScriptableRendererFeature feature = rendererData.rendererFeatures.Find(item => item is ScreenFadeFeature);

        if(feature is ScreenFadeFeature screenFade)
        {
            fadeMaterial = Instantiate(screenFade.settings.material);
            screenFade.settings.runtimeMaterial = fadeMaterial;
        }

    }

    public void FadeIn(UnityAction onComplete) 
    {
        fadeMaterial.DOFloat(1f, "_Alpha", duration).OnComplete(() =>
        {
            if (onComplete != null)
            {
                onComplete.Invoke();
            }
        });
    }

    public void FadeOut(UnityAction onComplete)
    {
        fadeMaterial.DOFloat(0f, "_Alpha", duration).OnComplete(() =>
        {
            if(onComplete != null)
            {
                onComplete.Invoke();
            }
        });
    }

}
