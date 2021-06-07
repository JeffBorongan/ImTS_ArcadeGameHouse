using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSavingManager : MonoBehaviour
{

    private void Start()
    {
        GameSettingData data = new GameSettingData();
        data.dataID = "Game Settings";
        data.spawnTime = 10;
        data.pointsToEarn = 20;

        Debug.Log("Local Data: " + JsonUtility.ToJson(data));
        SaveLocalData(data);

        GameSettingData localData = GetLocalData<GameSettingData>("Game Settings");
        Debug.Log("Retrieved Data: " + JsonUtility.ToJson(localData));
    }

    public void SaveLocalData(SaveData data)
    {
        PlayerPrefs.SetString(data.dataID, JsonUtility.ToJson(data));
    }

    public T GetLocalData<T>(string id)
    {
        return JsonUtility.FromJson<T>(PlayerPrefs.GetString(id));
    }

}

public class SaveData 
{
    public string dataID = "";
}

public class GameSettingData : SaveData 
{
    public int spawnTime = 0;
    public int pointsToEarn = 0;
}