using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationShopManager : MonoBehaviour
{
    public static CustomizationShopManager Instance { private set; get; }

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

    [SerializeField] private List<BodyPartCustomization> bodyPartCustomization = new List<BodyPartCustomization>();

    public List<BodyPartCustomization> BodyPartCustomization { get => bodyPartCustomization; }

    public bool CanBePurchased(BodyPartCustomizationProfile profile)
    {
        return UserDataManager.Instance.UserData.currentStarsObtained >= profile.starCost;
    }

    public bool IsPurchased(BodyPartCustomizationProfile profile)
    {
        return UserDataManager.Instance.PurchasedHistory.Contains(profile.skinID);
    }

    public void Buy(BodyPartCustomizationProfile profile)
    {
        if (!IsPurchased(profile))
        {
            UserDataManager.Instance.AddPurchase(profile);
        }
    }
}

[System.Serializable]
public class BodyPartCustomization
{
    public BodyPartID bodyPartID = BodyPartID.BOOTS;
    public List<BodyPartCustomizationProfile> bodyPartProfile = new List<BodyPartCustomizationProfile>();
}
