using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ElevatorManager : MonoBehaviour
{
    #region Singleton

    public static ElevatorManager Instance { private set; get; }

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

    #endregion

    #region Parameters

    [SerializeField] private GameObject elevatorDoorLeft = null;
    [SerializeField] private GameObject elevatorDoorRight = null;
    [SerializeField] private AudioSource elevatorAudioSource = null;
    [SerializeField] private AudioClip elevatorDoorClip = null;
    [SerializeField] private List<GameObject> disableObjects = new List<GameObject>();
    [SerializeField] private List<Button> elevatorButtons = new List<Button>();
    private MeshRenderer elevatorPart;
    private bool isDoorOpen = false;
    private bool closeDoorDetection = false;
    private bool playerDetection = false;
    private int elementNumber;

    #endregion

    #region Encapsulations

    public List<GameObject> DisableObjects { get => disableObjects; }
    public bool CloseDoorDetection { get => closeDoorDetection; set => closeDoorDetection = value; }
    public bool PlayerDetection { get => playerDetection; set => playerDetection = value; }

    #endregion

    #region Starting Floor

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)Floors.SpaceLobby, LoadSceneMode.Additive);
    }

    #endregion

    #region Floor Buttons Enabled

    public void EnableFloorButton(int floorNumber, bool interact)
    {
        floorNumber--;
        if (floorNumber < 4 && floorNumber >= 0)
        {
            elevatorButtons[floorNumber].gameObject.SetActive(interact);
        }
    }

    #endregion

    #region Function with Delay

    private IEnumerator FunctionWithDelay(float waitTime, UnityAction function)
    {
        yield return new WaitForSeconds(waitTime);
        function.Invoke();
    }

    #endregion

    #region Elevator Functions

    public void OnPressedElevatorButton(int floor)
    {
        CloseElevatorDoorAndLoad(() => 
        {
            SceneManager.Instance.LoadFloor((Floors)floor, () =>
            {
                StartCoroutine(FunctionWithDelay(3f, () => OpenElevatorDoor()));
            });
        });
    }

    public void OpenElevatorDoor()
    {
        foreach (var button in elevatorButtons)
        {
            button.interactable = false;
        }

        if (!isDoorOpen)
        {
            isDoorOpen = true;
            OpenDoor(() => 
            {
                DisableEmissive();
            });  
        } 
        else
        {
            StartCoroutine(FunctionWithDelay(2f, () => DisableEmissive()));
        }
    }

    public void CloseElevatorDoor()
    {
        foreach (var button in elevatorButtons)
        {
            button.interactable = false;
        }

        if (isDoorOpen)
        {
            CloseDoor(() => 
            {
                DisableEmissive();
            });
            isDoorOpen = false;
        }
        else
        {
            StartCoroutine(FunctionWithDelay(2f, () => DisableEmissive()));
        }
    }

    private void CloseElevatorDoorAndLoad(UnityAction onComplete)
    {
        foreach (var button in elevatorButtons)
        {
            button.interactable = false;
        }

        if (isDoorOpen)
        {
            CloseDoor(() =>
            {
                onComplete.Invoke();
            });
            isDoorOpen = false;
        }
        else
        {
            onComplete.Invoke();
        }
    }

    private void OpenDoor(UnityAction onComplete)
    {
        elevatorAudioSource.PlayOneShot(elevatorDoorClip, 1f);
        elevatorDoorLeft.transform.DOMoveX(-1.03f, 2f).OnComplete(onComplete.Invoke);
        elevatorDoorRight.transform.DOMoveX(0.8976f, 2f).OnComplete(onComplete.Invoke);
    }

    private void CloseDoor(UnityAction onComplete)
    {
        elevatorAudioSource.PlayOneShot(elevatorDoorClip, 1f);
        elevatorDoorLeft.transform.DOMoveX(-0.48f, 2f).OnComplete(onComplete.Invoke);
        elevatorDoorRight.transform.DOMoveX(0.3428232f, 2f).OnComplete(onComplete.Invoke);
    }

    public void GetElevatorPart(MeshRenderer meshRenderer)
    {
        elevatorPart = meshRenderer;
    }

    public void EnableEmissive(int number)
    {
        elementNumber = number;
        elevatorPart.materials[elementNumber].EnableKeyword("_EMISSION");
    }

    private void DisableEmissive()
    {
        elevatorPart.materials[elementNumber].DisableKeyword("_EMISSION");
        foreach (var button in elevatorButtons)
        {
            button.interactable = true;
        }
    }

    #endregion
}