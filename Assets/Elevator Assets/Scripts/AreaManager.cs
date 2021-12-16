using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaManager : MonoBehaviour
{
    public static AreaManager Instance { private set; get; }
    public LocationEvent OnPlayerExitEvent = new LocationEvent();
    public LocationEvent OnPlayerEnterEvent = new LocationEvent();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public string playerLocation;

    public void OnPlayerEnter(string AreaName)
    {
        playerLocation = AreaName;
        OnPlayerEnterEvent.Invoke(AreaName);
    }

    public void OnPlayerExit(string AreaName)
    {
        OnPlayerExitEvent.Invoke(AreaName);
        if (AreaName == "Elevator")
        {
            ElevatorFloorManager.Instance.closeElevatorDoor();
        }
    }
}

public class LocationEvent : UnityEvent<string>
{ 

}