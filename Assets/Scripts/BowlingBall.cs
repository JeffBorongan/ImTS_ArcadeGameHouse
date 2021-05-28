using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BowlingBall : MonoBehaviour
{
    private BowlingBallStates currentState = BowlingBallStates.OnGround;
    private XRGrabInteractable interactable;
    private Rigidbody rigidBody;
    private int hitCounter = 0;
    public BowlingBallStates CurrentState { get => currentState; }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        interactable = GetComponent<XRGrabInteractable>();
        interactable.selectEntered.AddListener(HandleSelectEntered);
        interactable.selectExited.AddListener(HandleSelectExited);
    }

    private void ChangeState(BowlingBallStates newState)
    {
        currentState = newState;
    }

    private void HandleSelectEntered(SelectEnterEventArgs arg0)
    {
        ChangeState(BowlingBallStates.OnHold);
    }

    private void HandleSelectExited(SelectExitEventArgs arg0)
    {
        ChangeState(BowlingBallStates.OnRelease);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnemyAlien")
        {
            Destroy(other.gameObject);
            hitCounter++;

            if (hitCounter == 3)
            {
                Destroy(gameObject);
            }
        }

        if (currentState == BowlingBallStates.OnRelease)
        {
            if (other.gameObject.tag == "BowlingLane")
            {
                ChangeState(BowlingBallStates.OnGround);
            }
        }
    }
}

public enum BowlingBallStates
{
    OnGround,
    OnHold,
    OnRelease
}