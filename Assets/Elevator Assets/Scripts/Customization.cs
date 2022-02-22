using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customization : MonoBehaviour
{
    [SerializeField] private GameObject pnlTryCustomize = null;
    [SerializeField] private GameObject pnlCategorySelection = null;
    [SerializeField] private GameObject pnlHelmetCustomization = null;
    [SerializeField] private GameObject pnlTorsoCustomization = null;
    [SerializeField] private GameObject pnlArmsCustomization = null;
    [SerializeField] private GameObject pnlShoesCustomization = null;
    [SerializeField] private GameObject pnlConfirmCustomization = null;

    [SerializeField] private Button btnCustomize = null;

    [SerializeField] private Button btnHelmetCategory = null;
    [SerializeField] private Button btnTorsoCategory = null;
    [SerializeField] private Button btnArmsCategory = null;
    [SerializeField] private Button btnShoesCategory = null;

    [SerializeField] private Button btnConfirm = null;

    private int enableHelmet = 6;
    private int enableTorso = 9;
    private int enableArm = 0;
    private int enableShoe = 3;

    private void Start()
    {
        btnCustomize.onClick.AddListener(() =>
        {
            pnlCategorySelection.SetActive(true);
            pnlConfirmCustomization.SetActive(true);
            pnlTryCustomize.SetActive(false);
        });

        btnHelmetCategory.onClick.AddListener(() => 
        {
            pnlHelmetCustomization.SetActive(true);
            pnlTorsoCustomization.SetActive(false);
            pnlArmsCustomization.SetActive(false);
            pnlShoesCustomization.SetActive(false);
        });

        btnTorsoCategory.onClick.AddListener(() =>
        {
            pnlTorsoCustomization.SetActive(true);
            pnlHelmetCustomization.SetActive(false);
            pnlArmsCustomization.SetActive(false);
            pnlShoesCustomization.SetActive(false);
        });

        btnArmsCategory.onClick.AddListener(() =>
        {
            pnlArmsCustomization.SetActive(true);
            pnlTorsoCustomization.SetActive(false);
            pnlHelmetCustomization.SetActive(false);
            pnlShoesCustomization.SetActive(false);
        });

        btnShoesCategory.onClick.AddListener(() =>
        {
            pnlShoesCustomization.SetActive(true);
            pnlArmsCustomization.SetActive(false);
            pnlTorsoCustomization.SetActive(false);
            pnlHelmetCustomization.SetActive(false);
        });

        btnConfirm.onClick.AddListener(() =>
        {
            for (int index = 0; index < BodyMeasurement.Instance.customizePart.Length; index++)
            {
                BodyMeasurement.Instance.customizePart[index].SetActive(false);
            }

            BodyMeasurement.Instance.customizePart[enableHelmet].SetActive(true);
            BodyMeasurement.Instance.customizePart[enableTorso].SetActive(true);
            BodyMeasurement.Instance.customizePart[enableArm].SetActive(true);
            BodyMeasurement.Instance.customizePart[enableShoe].SetActive(true);

            pnlTryCustomize.SetActive(true);
            pnlCategorySelection.SetActive(false);
            pnlHelmetCustomization.SetActive(false);
            pnlTorsoCustomization.SetActive(false);
            pnlArmsCustomization.SetActive(false);
            pnlShoesCustomization.SetActive(false);
            pnlConfirmCustomization.SetActive(false);
        });
    }

    public void AddHelmetPart(int enable)
    {
        enableHelmet = enable;
    }

    public void AddTorsoPart(int enable)
    {
        enableTorso = enable;
    }

    public void AddArmPart(int enable)
    {
        enableArm = enable;
    }

    public void AddShoePart(int enable)
    {
        enableShoe = enable;
    }
}