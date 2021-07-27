using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RoomInteraction : MonoBehaviour
{
    public static RoomInteraction Instance { private set; get; }

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

    [SerializeField] private XRRayInteractor leftInteractor = null;
    [SerializeField] private XRRayInteractor rightInteractor = null;
    private EnvironmentPoints currentPoint = EnvironmentPoints.AvatarRoomMainCenter;

    public EnvironmentPoints CurrentPoint { get => currentPoint; }

    private void Start()
    {
        leftInteractor.selectEntered.AddListener(HandleOnSelect);
        rightInteractor.selectEntered.AddListener(HandleOnSelect);
    }

    private void HandleOnSelect(SelectEnterEventArgs newEvent)
    {
        DoorTarget door = newEvent.interactable.GetComponent<DoorTarget>();

        if(door != null)
        {
            door.EnterDoor(transform);
            currentPoint = door.Point;
        }
    }
}
