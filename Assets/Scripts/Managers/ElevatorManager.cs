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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Parameters

    [SerializeField] private GameObject elevatorPrefab = null;
    [SerializeField] private GameObject elevatorDoorLeft = null;
    [SerializeField] private GameObject elevatorDoorRight = null;
    [SerializeField] private AudioSource elevatorAudioSource = null;
    [SerializeField] private AudioClip doorOpenClip = null;
    [SerializeField] private AudioClip doorCloseClip = null;
    [SerializeField] private List<Button> elevatorButtons = new List<Button>();
    private MeshRenderer elevatorPart;
    private bool isDoorOpen = false;
    private int elementNumber;

    #endregion

    #region Encapsulations

    public GameObject ElevatorPrefab { get => elevatorPrefab; }

    #endregion

    #region Starting Floor

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)floor.SpaceLobby, LoadSceneMode.Additive);
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
            SceneManager.Instance.LoadFloor((floor)floor, () =>
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
        elevatorAudioSource.PlayOneShot(doorOpenClip, 1f);
        elevatorDoorLeft.transform.DOMoveX(-1.03f, 2f).OnComplete(onComplete.Invoke);
        elevatorDoorRight.transform.DOMoveX(0.8976f, 2f).OnComplete(onComplete.Invoke);
    }

    private void CloseDoor(UnityAction onComplete)
    {
        elevatorAudioSource.PlayOneShot(doorCloseClip, 1f);
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