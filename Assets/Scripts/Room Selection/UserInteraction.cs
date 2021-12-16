using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.InputSystem.InputAction;

public class UserInteraction : MonoBehaviour
{
    public static UserInteraction Instance { private set; get; }

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

        toggleHelmet.action.performed += ToggleHelmetUI;
    }

    private void OnDestroy()
    {
        toggleHelmet.action.performed -= ToggleHelmetUI;
    }

    [SerializeField] private Transform cameraTransform = null;
    [SerializeField] private XRRayInteractor leftInteractor = null;
    [SerializeField] private XRRayInteractor rightInteractor = null;
    [SerializeField] private InputActionReference toggleHelmet = null;
    [SerializeField] private AudioSource audioSourceAnnouncer = null;

    public RoomIDEvent OnChangeRoom = new RoomIDEvent();
    private RoomID currentRoom = RoomID.AvatarRoomMain;

    private EnvironmentPoints currentPoint = EnvironmentPoints.AvatarRoomMainCenter;

    public EnvironmentPoints CurrentPoint { get => currentPoint; }

    private void Start()
    {
        leftInteractor.selectEntered.AddListener(HandleOnSelect);

        rightInteractor.selectEntered.AddListener(HandleOnSelect);

        OnChangeRoom.Invoke(currentRoom);
    }

    private void HandleOnSelect(SelectEnterEventArgs newEvent)
    {
        Door door = newEvent.interactableObject.transform.GetComponent<Door>();

        if(door != null)
        {
            door.EnterRoom(null, () => 
            {
                SetCurrentPoint(door.DestinationPoint);
            });
        }
    }



    public void SetCurrentPoint(EnvironmentPoints newPoint)
    {
        currentPoint = newPoint;
    }

    public void SetRoomInteraction(bool enable)
    {
        leftInteractor.gameObject.SetActive(enable);
        rightInteractor.gameObject.SetActive(enable);
    }

    private void ToggleHelmetUI(CallbackContext context)
    {
        CharacterHelmet.Instance.ToggleHelmetUI();
    }

    public void Teleport(Vector3 destination)
    {
        transform.position = destination - new Vector3(-cameraTransform.localPosition.z, 0f, cameraTransform.localPosition.x);
    }

    public void SetCurrentRoom(RoomID id)
    {
        currentRoom = id;
        OnChangeRoom.Invoke(currentRoom);
    }

    public void SetCurrentRoomInt(int roomID)
    {
        currentRoom = (RoomID)roomID;
        OnChangeRoom.Invoke(currentRoom);
    }

    public void AnnounceToPlayer(AudioClip source, bool announce)
    {
        if (announce)
        {
            audioSourceAnnouncer.clip = source;
            audioSourceAnnouncer.Play();
        }
        else
        {
            audioSourceAnnouncer.Stop();
        }
    }
}

public class RoomIDEvent : UnityEvent<RoomID> { }