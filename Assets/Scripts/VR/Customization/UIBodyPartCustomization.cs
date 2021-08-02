using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBodyPartCustomization : MonoBehaviour
{    public static UIBodyPartCustomization Instance { private set; get; }

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

    [SerializeField] private TextMeshProUGUI txtLabel = null;
    [SerializeField] private TextMeshProUGUI txtCurrency = null;
    [SerializeField] private GameObject pnlBuyOption = null;
    [SerializeField] private Button btnBuy = null;
    [SerializeField] private List<UIBodyPartCustom> bodyPartsCustom = new List<UIBodyPartCustom>();

    private void Start()
    {
        GameExecution.Instance.OnEventRegistration.AddListener(() =>
        {
            UserDataManager.Instance.OnUserDataUpdate.AddListener(HandleOnUserDataUpdate);
        });
    }

    private void HandleOnUserDataUpdate(UserData data)
    {
        txtCurrency.text = data.currentStarsObtained.ToString();
    }

    public void OpenBodyPartSelections(CharacterCustomization character, CharacterCustomization characterMimic, BodyPartID id)
    {
        txtLabel.text = id.ToString();

        BodyPart bodyPart = character.bodyParts.Where(b => b.id == id).FirstOrDefault();
        BodyPart bodyPartMimic = characterMimic.bodyParts.Where(b => b.id == id).FirstOrDefault();

        foreach (var ui in bodyPartsCustom)
        {
            ui.gameObject.SetActive(false);
        }

        BodyPartCustomization customizationBodyPart = CustomizationShopManager.Instance.BodyPartCustomization.Where(b => b.bodyPartID == id).FirstOrDefault();

        for (int i = 0; i < customizationBodyPart.bodyPartProfile.Count; i++)
        {
            bodyPartsCustom[i].imgBodyPartPreview.sprite = customizationBodyPart.bodyPartProfile[i].bodyPartSpriteUI;
            bodyPartsCustom[i].txtStarCost.text = customizationBodyPart.bodyPartProfile[i].starCost.ToString();

            if (CustomizationShopManager.Instance.IsPurchased(customizationBodyPart.bodyPartProfile[i]))
            {
                bodyPartsCustom[i].imgLocked.gameObject.SetActive(false);
                bodyPartsCustom[i].txtStarCost.gameObject.SetActive(false);
            }
            else
            {
                bodyPartsCustom[i].imgLocked.gameObject.SetActive(true);
                bodyPartsCustom[i].txtStarCost.gameObject.SetActive(true);
            }

            bodyPartsCustom[i].btnBodyPartCustom.onClick.RemoveAllListeners();
            BodyPartCustomizationProfile profile = customizationBodyPart.bodyPartProfile[i];
            bodyPartsCustom[i].btnBodyPartCustom.onClick.AddListener(() =>
            {
                Material newMaterial = profile.bodyPartMaterial;

                if (CustomizationShopManager.Instance.IsPurchased(profile)) 
                {
                    bodyPart.ChangeMaterial(newMaterial);
                    bodyPartMimic.ChangeMaterial(newMaterial);
                }
                else if(CustomizationShopManager.Instance.CanBePurchased(profile))
                {
                    pnlBuyOption.SetActive(true);

                    btnBuy.onClick.RemoveAllListeners();
                    btnBuy.onClick.AddListener(() => 
                    {
                        CustomizationShopManager.Instance.Buy(profile);
                        pnlBuyOption.SetActive(false);
                    });
                }
            });

            bodyPartsCustom[i].gameObject.SetActive(true);
        }
    }
}
