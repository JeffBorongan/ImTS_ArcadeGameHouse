using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            door.EnterDoor(transform, cameraTransform, ()=>
            {
                currentPoint = door.DestinationPoint;
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
}
