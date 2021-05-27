using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MomentomBall : MonoBehaviour
{
    [SerializeField] private float releaseForce = 10f;
    [SerializeField] private float momentomForce = 10f;

    private BowlingBallStates currentState = BowlingBallStates.OnGround;

    XRGrabInteractable interactable = null;
    private GameObject handInteracted = null;
    private Vector3 torgeReference = Vector3.zero;

    private Rigidbody rigid = null;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        interactable = GetComponent<XRGrabInteractable>();

        interactable.selectEntered.AddListener(HandleOnEnter);
        interactable.selectExited.AddListener(HandleOnExit);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(currentState != BowlingBallStates.OnRelease) { return; }

        if(collision.gameObject.tag == "BowlingLane")
        {
            rigid.AddTorque(torgeReference * momentomForce, ForceMode.Impulse);
        }
    }

    private void HandleOnExit(SelectExitEventArgs exitEvent)
    {
        rigid.AddForce(handInteracted.transform.up * releaseForce, ForceMode.Impulse);
        torgeReference = handInteracted.transform.right;

        currentState = BowlingBallStates.OnRelease;
    }

    private void HandleOnEnter(SelectEnterEventArgs enterEvent)
    {
        handInteracted = enterEvent.interactor.gameObject;
        currentState = BowlingBallStates.OnHold;
    }
}
