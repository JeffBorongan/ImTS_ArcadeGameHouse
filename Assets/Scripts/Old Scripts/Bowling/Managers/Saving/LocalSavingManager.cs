using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSavingManager : MonoBehaviour
{
    public static LocalSavingManager Instance { private set; get; }

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

    public void SaveLocalData(SaveData data)
    {
        PlayerPrefs.SetString(data.dataID, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    public T GetLocalData<T>(string id)
    {
        return JsonUtility.FromJson<T>(PlayerPrefs.GetString(id));
    }

    public bool IsLocalDataStored(string id)
    {
        return PlayerPrefs.HasKey(id);
    }

    public void DeleteLocalData(string id)
    {
        PlayerPrefs.DeleteKey(id);
    }

}

public class SaveData 
{
    public string dataID = "";
}

public class SpaceBowlingSaveData : SaveData
{
    public float enemySpawnIntervalValue = 0f;
    public float enemySpeedValue = 0f;
    public int pointsToEarnValue = 0;
    public int numberOfFailsValue = 0;
    public float dispenserOffsetValue = 0f;
    public string lanes = "";
}

public class UserData : SaveData
{
    public int currentStarsObtained = 0;
    public string purchasedHistory = "Boots_Blue,Gloves_Blue,Helmet_Blue,Jetpack_Blue,JointPads_Blue,Suit_Blue,Vest_Blue,Wristband_Blue,";
}