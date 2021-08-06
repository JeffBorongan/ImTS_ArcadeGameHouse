using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance { private set; get; }

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

    [SerializeField] private int startPerPoint = 30;

    public UserDataEvent OnUserDataUpdate = new UserDataEvent();

    private UserData userData = null;
    public UserData UserData { get => userData; }

    private List<string> purchasedHistory = new List<string>();
    public List<string> PurchasedHistory { get => purchasedHistory; }

    private void Start()
    {
        if (LocalSavingManager.Instance.IsLocalDataStored("User"))
        {
            userData = LocalSavingManager.Instance.GetLocalData<UserData>("User");
        }
        else
        {
            userData = new UserData(); 
            SaveUserData();
        }

        purchasedHistory = userData.purchasedHistory.Split(',').ToList();

        OnUserDataUpdate.Invoke(userData);
    }

    private void SaveUserData()
    {
        LocalSavingManager.Instance.SaveLocalData(userData);
        OnUserDataUpdate.Invoke(userData);
    }

    public void AddStars(int points)
    {
        userData.currentStarsObtained += Mathf.RoundToInt(points / startPerPoint);
        SaveUserData();
    }

    public void AddPurchase(BodyPartCustomizationProfile profile)
    {
        if (!purchasedHistory.Contains(profile.skinID))
        {
            purchasedHistory.Add(profile.skinID);

            userData.purchasedHistory = "";

            foreach (var purchased in purchasedHistory)
            {
                userData.purchasedHistory += purchased + ",";
            }

            userData.currentStarsObtained -= profile.starCost;
        }

        SaveUserData();
    }
}

public class UserDataEvent : UnityEvent<UserData> { }
