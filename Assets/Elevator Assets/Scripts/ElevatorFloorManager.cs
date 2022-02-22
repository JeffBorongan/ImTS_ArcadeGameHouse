using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

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

    public GameObject characterPrefab;
    public GameObject characterSuit;
    public GameObject characterCamera;

    public GameObject elevatorPrefab;
    public GameObject elevatorDoorLeft;
    public GameObject elevatorDoorRight;
    public AudioSource elevatorSFX;
    public AudioClip elevatorSFXOpen;
    public AudioClip elevatorSFXClose;
    public Button[] elevatorButtons;
    public float doorOpeningDelay;
    public float doorClosingDelay = 2f;
    public bool isDoorClosing = false;
    public bool isDoorOpen = false;

    private MeshRenderer elevatorPart;
    private int elementNumber;

    void Start()
    {
        SceneManager.LoadScene(((int)floor.SpaceLobby), LoadSceneMode.Additive);
    }

    public void onPressedElevatorButton(int floor)
    {
        closeElevatorDoor(() => {
            SceneManagement.Instance.LoadFloor((floor)floor, () =>
            {
                Invoke("openElevatorDoor", 3f);
            });
        });
    }

    public void openElevatorDoor()
    {
        foreach (var button in elevatorButtons)
        {
            button.interactable = false;
        }

        if (!isDoorOpen)
        {
            isDoorOpen = true;
            isDoorClosing = false;
            openDoor(() => 
            {
                disableEmissive();
            });  
        } 
        else
        {
            Invoke("disableEmissive", 2f);
        }
    }

    public void closeElevatorDoor()
    {
        foreach (var button in elevatorButtons)
        {
            button.interactable = false;
        }

        if (isDoorOpen)
        {
            closeDoor(() => 
            {
                isDoorClosing = false;
                disableEmissive();
            });
            isDoorClosing = true;
            isDoorOpen = false;
        }
        else
        {
            Invoke("disableEmissive", 2f);
        }
    }
    public void closeElevatorDoor(UnityAction onComplete)
    {
        foreach (var button in elevatorButtons)
        {
            button.interactable = false;
        }

        if (isDoorOpen)
        {
            closeDoor(() =>
            {
                isDoorClosing = false;
                onComplete.Invoke();
            });
            isDoorClosing = true;
            isDoorOpen = false;
        }
        else
        {
            onComplete.Invoke();
        }
    }

    public void openDoor(UnityAction onComplete)
    {
        elevatorSFX.PlayOneShot(elevatorSFXOpen, 1f);
        elevatorDoorLeft.transform.DOMoveX(-1.03f, 2f).OnComplete(onComplete.Invoke).SetDelay(doorOpeningDelay);
        elevatorDoorRight.transform.DOMoveX(0.8976f, 2f).OnComplete(onComplete.Invoke).SetDelay(doorOpeningDelay);
    }

    public void closeDoor(UnityAction onComplete)
    {
        elevatorSFX.PlayOneShot(elevatorSFXClose, 1f);
        elevatorDoorLeft.transform.DOMoveX(-0.48f, 2f).OnComplete(onComplete.Invoke).SetDelay(doorClosingDelay);
        elevatorDoorRight.transform.DOMoveX(0.3428232f, 2f).OnComplete(onComplete.Invoke).SetDelay(doorClosingDelay);
    }

    public void getElevatorPart(MeshRenderer meshRenderer)
    {
        elevatorPart = meshRenderer;
    }

    public void enableEmissive(int number)
    {
        elementNumber = number;
        elevatorPart.materials[elementNumber].EnableKeyword("_EMISSION");
    }

    private void disableEmissive()
    {
        elevatorPart.materials[elementNumber].DisableKeyword("_EMISSION");
        foreach (var button in elevatorButtons)
        {
            button.interactable = true;
        }
    }
}