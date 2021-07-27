using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UXCustomization : MonoBehaviour
{
    [Header("VR Components")]
    [SerializeField] private Transform vrCameraPoint = null;
    [SerializeField] private CharacterCustomization character = null;
    [SerializeField] private Transform characterMimic = null;
    [SerializeField] private GameObject pnlCustomization = null;

    [Header("Message")]
    [SerializeField] private TextMeshProUGUI txtMessage = null;

    [Header("Capture Height")]
    [SerializeField] private string captureHeightMessage = "";
    [SerializeField] private float captureSensitivity = 0.01f;
    [SerializeField] private float captureProgressSentivity = 10f;
    [SerializeField] private Slider progressBar = null;

    public void ShowMessage(string message, float duration, UnityAction OnComplete)
    {
        txtMessage.text = message;

        txtMessage.DOFade(1f, 0.2f).OnComplete(() =>
        {
            txtMessage.DOFade(0f, 0.2f).SetDelay(duration).OnComplete(OnComplete.Invoke);
        });
    }

    public void StartUX()
    {
        ShowMessage(captureHeightMessage, 5f, () =>
        {
            progressBar.gameObject.SetActive(true);
            StartCoroutine(HeightCaptureCour(() => 
            {
                pnlCustomization.SetActive(true);
            }));
        });
    }

    IEnumerator HeightCaptureCour(UnityAction OnEndAction)
    {
        yield return new WaitForSeconds(1f);

        Vector3 headPos = vrCameraPoint.position;
        float progress = 0f;
        while (progress < 100f)
        {
            Vector3 headMagnitude = vrCameraPoint.position - headPos;
            headPos = vrCameraPoint.position;

            if (headMagnitude.magnitude < captureSensitivity)
            {
                progress += Time.deltaTime * captureProgressSentivity;
            }
            else
            {
                progress = 0f;
            }

            progressBar.value = progress / 100;
            yield return new WaitForEndOfFrame();
        }

        progressBar.gameObject.SetActive(false);
        AdjustHeight();
        OnEndAction.Invoke();
    }

    private void AdjustHeight()
    {
        character.gameObject.SetActive(true);
        character.SetHeight();

        if (!characterMimic.gameObject.activeSelf)
        {
            characterMimic.transform.localScale = character.transform.localScale;
            characterMimic.gameObject.SetActive(true);
        }
    }
}
