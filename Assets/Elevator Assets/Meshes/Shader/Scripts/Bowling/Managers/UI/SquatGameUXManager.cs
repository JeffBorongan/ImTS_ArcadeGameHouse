using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class SquatGameUXManager : MonoBehaviour
{
    public static SquatGameUXManager Instance { private set; get; }

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

    [Header("Parameters")]
    [SerializeField] private Slider pullUpHeight = null;
    [SerializeField] private TMP_Text txtPullUpHeight = null;
    [SerializeField] private Slider pushDownHeight = null;
    [SerializeField] private TMP_Text txtPushDownHeight = null;

    [SerializeField] private Button btnUpdate = null;

    private float pullUpHeightValue = 1f;
    private float pushDownHeightValue = 0.5f;

    private void Start()
    {
        XRSettings.gameViewRenderMode = GameViewRenderMode.OcclusionMesh;

        btnUpdate.onClick.AddListener(HandleOnUpdate);

        pullUpHeight.onValueChanged.AddListener(HandleOnChangePullUpHeight);
        pushDownHeight.onValueChanged.AddListener(HandleOnChangePushDownHeight);

        SetUIWithDefaultValues();
    }

    private void SetUIWithDefaultValues()
    {
        pullUpHeightValue = Mathf.Round(pullUpHeightValue * 100f) / 100f;
        pushDownHeightValue = Mathf.Round(pushDownHeightValue * 100f) / 100f;

        pullUpHeight.value = pullUpHeightValue;
        pushDownHeight.value = pushDownHeightValue;
    }

    private void HandleOnChangePullUpHeight (float value)
    {
        value = Mathf.Round(value * 100f) / 100f;
        txtPullUpHeight.text = value.ToString();
        btnUpdate.interactable = true;
    }

    private void HandleOnChangePushDownHeight(float value)
    {
        value = Mathf.Round(value * 100f) / 100f;
        txtPushDownHeight.text = value.ToString();
        btnUpdate.interactable = true;
    }

    public void HandleOnStart()
    {
        pullUpHeightValue = pullUpHeight.value;
        pushDownHeightValue = pushDownHeight.value;

        pullUpHeightValue = Mathf.Round(pullUpHeightValue * 100f) / 100f;
        pushDownHeightValue = Mathf.Round(pushDownHeightValue * 100f) / 100f;

        SquatGameManagement.Instance.sessionData.pullUpHeight = pullUpHeightValue;
        SquatGameManagement.Instance.sessionData.pushDownHeight = pushDownHeightValue;
    }

    private void HandleOnUpdate()
    {
        pullUpHeightValue = pullUpHeight.value;
        pushDownHeightValue = pushDownHeight.value;

        pullUpHeightValue = Mathf.Round(pullUpHeightValue * 100f) / 100f;
        pushDownHeightValue = Mathf.Round(pushDownHeightValue * 100f) / 100f;

        SquatGameManagement.Instance.sessionData.pullUpHeight = pullUpHeightValue;
        SquatGameManagement.Instance.sessionData.pushDownHeight = pushDownHeightValue;

        btnUpdate.interactable = false;
    }
}