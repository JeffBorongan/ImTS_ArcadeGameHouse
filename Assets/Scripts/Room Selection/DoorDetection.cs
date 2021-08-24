using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorDetection : MonoBehaviour
{
    [SerializeField] private XRRayInteractor handRayInteractor = null;
    [SerializeField] private TextMeshProUGUI txtRoomName = null;

    private void Start()
    {
        handRayInteractor.hoverEntered.AddListener(HandleOnHoverEnter);
        handRayInteractor.hoverExited.AddListener(HandleOnHoverExit);
    }

    private void HandleOnHoverExit(HoverExitEventArgs hoverEvent)
    {
        txtRoomName.text = "";
    }

    private void HandleOnHoverEnter(HoverEnterEventArgs hoverEvent)
    {
        Door target = hoverEvent.interactable.GetComponent<Door>();

        if(target != null)
        {
            txtRoomName.text = target.RoomName;
        }
    }
}
