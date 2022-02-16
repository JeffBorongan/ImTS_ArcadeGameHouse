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
    public static UXCustomization Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [Header("VR Components")]
    [SerializeField] private Transform vrCameraPoint = null;
    [SerializeField] private Transform vrLeftHandPoint = null;
    [SerializeField] private Transform vrRightHandPoint = null;
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

    public DictionaryEvent OnUpdateAnatomy = new DictionaryEvent();

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
            StartCoroutine(BodyAnatomyCapture(() => 
            {
                pnlCustomization.transform.position = new Vector3(pnlCustomization.transform.position.x, vrCameraPoint.transform.position.y, pnlCustomization.transform.position.z);
                pnlCustomization.SetActive(true);

                Dictionary<string, Vector3> currentAnatomy = new Dictionary<string, Vector3>();
                currentAnatomy.Add(AnatomyPart.Head.ToString(), vrCameraPoint.position);
                currentAnatomy.Add(AnatomyPart.LeftHand.ToString(), vrLeftHandPoint.localPosition);
                currentAnatomy.Add(AnatomyPart.RightHand.ToString(), vrRightHandPoint.localPosition);

                OnUpdateAnatomy.Invoke(currentAnatomy);
            }));
        });
    }

    IEnumerator BodyAnatomyCapture(UnityAction OnEndAction)
    {
        yield return new WaitForSeconds(1f);

        Vector3 headPos = vrCameraPoint.position;
        Vector3 leftHandPos = vrLeftHandPoint.position;
        Vector3 rightHandPos = vrRightHandPoint.position;
        float progress = 0f;

        while (progress < 100f)
        {
            Vector3 headMagnitude = vrCameraPoint.position - headPos;
            Vector3 leftHandMagnitude = vrLeftHandPoint.position - leftHandPos;
            Vector3 rightHandMagnitude = vrRightHandPoint.position - rightHandPos;

            headPos = vrCameraPoint.position;
            leftHandPos = vrLeftHandPoint.position;
            rightHandPos = vrRightHandPoint.position;

            if (headMagnitude.magnitude < captureSensitivity && leftHandMagnitude.magnitude < captureSensitivity && rightHandMagnitude.magnitude < captureSensitivity)
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

public class DictionaryEvent : UnityEvent<Dictionary<string, Vector3>> { }

public enum AnatomyPart
{
    Head,
    LeftHand,
    RightHand
}