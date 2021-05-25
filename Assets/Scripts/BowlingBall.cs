using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BowlingBall : MonoBehaviour
{
    private XRGrabInteractable interactable;
    private Rigidbody rigidBody;
    public BowlingBallStates currentState = BowlingBallStates.OnGround;
    public float movementSpeed = 10;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        interactable = GetComponent<XRGrabInteractable>();
        interactable.onSelectExited.AddListener(HandleOnExit);
    }

    private void HandleOnExit(XRBaseInteractor arg0)
    {
        changeState(BowlingBallStates.OnRelease);
    }

    public void OnCollisionEnter(Collision other)
    {
        if(currentState == BowlingBallStates.OnRelease)
        {
            AddBowlMomentum(other);
        }
    }

    public void changeState(BowlingBallStates newState)
    {
        currentState = newState;
    }

    public void AddBowlMomentum(Collision other)
    {
        if (other.gameObject.tag == "BowlingLane")
        {
            rigidBody.AddForce(transform.forward * movementSpeed, ForceMode.Impulse);
            Debug.Log("Add Force");
            //rigidBody.velocity = transform.forward * 1500 * Time.fixedDeltaTime;
            changeState(BowlingBallStates.OnGround);
        }
    }
}

public enum BowlingBallStates
{
    OnGround,
    OnHold,
    OnRelease
}
