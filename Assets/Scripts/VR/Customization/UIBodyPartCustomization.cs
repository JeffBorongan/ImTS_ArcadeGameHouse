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
    [SerializeField] private List<UIBodyPartCustom> bodyPartsCustom = new List<UIBodyPartCustom>();

    private BodyPartCustomization currentSelectedBodyPart = null;

    private void Start()
    {
        GameExecution.Instance.OnEventRegistration.AddListener(() =>
        {
            UserDataManager.Instance.OnUserDataUpdate.AddListener(HandleOnUserDataUpdate);
        });
    }

    private void HandleOnUserDataUpdate(UserData data)
    {
        if(currentSelectedBodyPart == null) { return; }

        for (int i = 0; i < currentSelectedBodyPart.bodyPartProfile.Count; i++)
        {
            bodyPartsCustom[i].imgBodyPartPreview.sprite = currentSelectedBodyPart.bodyPartProfile[i].bodyPartSpriteUI;
            bodyPartsCustom[i].txtStarCost.text = currentSelectedBodyPart.bodyPartProfile[i].starCost.ToString();

            if (CustomizationShopManager.Instance.IsPurchased(currentSelectedBodyPart.bodyPartProfile[i]))
            {
                bodyPartsCustom[i].imgLocked.gameObject.SetActive(false);
                bodyPartsCustom[i].currency.SetActive(false);
            }
            else
            {
                bodyPartsCustom[i].imgLocked.gameObject.SetActive(true);
                bodyPartsCustom[i].currency.SetActive(true);
            }
        }
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

        currentSelectedBodyPart = CustomizationShopManager.Instance.BodyPartCustomization.Where(b => b.bodyPartID == id).FirstOrDefault();

        for (int i = 0; i < currentSelectedBodyPart.bodyPartProfile.Count; i++)
        {
            bodyPartsCustom[i].imgBodyPartPreview.sprite = currentSelectedBodyPart.bodyPartProfile[i].bodyPartSpriteUI;
            bodyPartsCustom[i].txtStarCost.text = currentSelectedBodyPart.bodyPartProfile[i].starCost.ToString();

            if (CustomizationShopManager.Instance.IsPurchased(currentSelectedBodyPart.bodyPartProfile[i]))
            {
                bodyPartsCustom[i].imgLocked.gameObject.SetActive(false);
                bodyPartsCustom[i].currency.SetActive(false);
            }
            else
            {
                bodyPartsCustom[i].imgLocked.gameObject.SetActive(true);
                bodyPartsCustom[i].currency.SetActive(true);
            }

            bodyPartsCustom[i].btnBodyPartCustom.onClick.RemoveAllListeners();
            BodyPartCustomizationProfile profile = currentSelectedBodyPart.bodyPartProfile[i];
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
                    Debug.Log("Purchase");
                    CustomizationShopManager.Instance.Buy(profile);
                    bodyPartsCustom[i].currency.SetActive(false);
                }
            });

            bodyPartsCustom[i].gameObject.SetActive(true);
        }
    }
}
