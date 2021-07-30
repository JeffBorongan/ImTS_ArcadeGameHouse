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

    private void Start()
    {
        GameExecution.Instance.OnInitialize.AddListener(() =>
        {
            List<BodyPartCustomization> vertical1 = new List<BodyPartCustomization>();
            List<BodyPartCustomization> vertical2 = new List<BodyPartCustomization>();

            for (int i = 0; i < bodyPartCustomization.Count; i++)
            {
                if (i < 4)
                {
                    vertical1.Add(bodyPartCustomization[i]);
                }
                else
                {
                    vertical2.Add(bodyPartCustomization[i]);
                }
            }

            UIBodyPartSelection.Instance.InitializeCustomization(vertical1, true);
            UIBodyPartSelection.Instance.InitializeCustomization(vertical2, false);
        });
    }

    public bool CanBePurchased(BodyPartCustomizationProfile profile)
    {
        return UserDataManager.Instance.UserData.currentStarsObtained > profile.starCost;
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
