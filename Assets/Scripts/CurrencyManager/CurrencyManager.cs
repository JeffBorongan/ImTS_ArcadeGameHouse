using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { private set; get; }

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

    private UserData userData = null;
    public UserData UserData { get => userData; }

    private void Start()
    {
        if (LocalSavingManager.Instance.IsLocalDataStored("User"))
        {
            userData = LocalSavingManager.Instance.GetLocalData<UserData>("User");
        }
        else
        {
            userData = new UserData();
            LocalSavingManager.Instance.SaveLocalData(userData);
        }
    }


    private void SaveUserData()
    {
        LocalSavingManager.Instance.SaveLocalData(userData);
    }

    public void AddStars(int points)
    {
        userData.currentStarsObtained += Mathf.RoundToInt(points / startPerPoint);
        SaveUserData();
    }
}
