using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterHelmet : MonoBehaviour
{
    public static CharacterHelmet Instance { private set; get; }

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

    [SerializeField] private TextMeshProUGUI txtVisorStarsUI = null;
    [SerializeField] private Transform topUIElement = null;
    [SerializeField] private Transform bottomUIElement = null;
    [SerializeField] private Ease ease = Ease.InOutQuad;
    private bool helmetUIEnable = true;

    private void Start()
    {
        UserDataManager.Instance.OnUserDataUpdate.AddListener(HandleOnUserDataUpdate);
        HandleOnUserDataUpdate(UserDataManager.Instance.UserData);
    }

    private void HandleOnUserDataUpdate(UserData newUserData)
    {
        txtVisorStarsUI.text = newUserData.currentStarsObtained + " Stars";
    }

    public void ToggleHelmetUI()
    {
        helmetUIEnable = !helmetUIEnable;
        topUIElement.DOScale(helmetUIEnable ? 1f : 0f, 0.5f).SetEase(ease);
        bottomUIElement.DOScale(helmetUIEnable ? 1f : 0f, 0.5f).SetEase(ease);
    }
}
