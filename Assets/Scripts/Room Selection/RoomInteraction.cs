using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RoomInteraction : MonoBehaviour
{
    [SerializeField] private XRRayInteractor leftInteractor = null;
    [SerializeField] private XRRayInteractor rightInteractor = null;

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
        }
    }
}
