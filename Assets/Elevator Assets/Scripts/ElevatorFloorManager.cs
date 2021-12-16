using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

public class ElevatorFloorManager : MonoBehaviour
{
    
    public static ElevatorFloorManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    } 

    public GameObject elevatorDoor;
    public GameObject elevatorButtons;
    public float doorOpeningDelay;
    public float doorClosingDelay = 2f;
    public bool isDoorClosing = false;
    public bool isDoorOpen = false;
    public bool isDoorBlocked;

    private Vector3 doorLoctaion;
    private string scene = "";
    private bool isLoaded;
    private bool canLoadScene;
   
    


    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        AreaManager.Instance.OnPlayerEnterEvent.AddListener(HandleOnPlayerEnter);
        AreaManager.Instance.OnPlayerEnterEvent.AddListener(HandleOnPlayerEnter);
    }

    private void HandleOnPlayerEnter(string AreaName)
    {
        if (AreaName == "Elevator")
        {
            closeElevatorDoor();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (AreaManager.Instance.playerLocation == "Elevator")
        {
            doorOpeningDelay = 10f;
        }
        else if (AreaManager.Instance.playerLocation == "Room")
        {
            doorOpeningDelay = 3f;
        }
    }

    public void onPressedElevatorButton(int floor)
    {
        if (!isDoorOpen)
        {
            SceneManagement.Instance.LoadFloor((floor)floor, () =>
               {
                   openElevatorDoor();
               });
        }
    }

    public void openElevatorDoor()
    {
        if (!isDoorOpen)
        {
            isDoorOpen = true;
            isDoorClosing = false;
            openDoor(() => 
            {
                elevatorButtons.SetActive(false);
                   
            });
            
        }
    }

    public void closeElevatorDoor()
    {
        if (isDoorOpen && !isDoorBlocked)
        {
           
            closeDoor(() => 
            {
                elevatorButtons.SetActive(true);
                isDoorClosing = false;
            });
            isDoorClosing = true;
            isDoorOpen = false;
        }
    }

    public void openDoor(UnityAction onComplete)
    {
        elevatorDoor.transform.DOMoveX(-1.2f, 2f).OnComplete(onComplete.Invoke).SetDelay(doorOpeningDelay);
    }

    public void closeDoor(UnityAction onComplete)
    {
        elevatorDoor.transform.DOMoveX(0f, 2f).OnComplete(onComplete.Invoke).SetDelay(doorClosingDelay);
    }


    void LoadScene()
    {
        if (!isLoaded)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }
    }
}
