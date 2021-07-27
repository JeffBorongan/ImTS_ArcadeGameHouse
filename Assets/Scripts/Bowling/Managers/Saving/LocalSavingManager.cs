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
    }

    public T GetLocalData<T>(string id)
    {
        return JsonUtility.FromJson<T>(PlayerPrefs.GetString(id));
    }

    public bool IsLocalDataStored(string id)
    {
        return PlayerPrefs.HasKey(id);
    }

}

public class SaveData 
{
    public string dataID = "";
}

public class SpaceBowlingSaveData : SaveData
{
    public float spawnTimeValue = 0f;
    public float alienMovementSpeedValue = 0f;
    public int pointPerAlienValue = 0;
    public int pointsToEarnValue = 0;
    public int aliensReachedTheCockpitValue = 0;
    public int timeToBeatValue = 0;
    public string lanes = "";
}

public class UserData : SaveData
{
    public int currentStarsObtained = 0;
}