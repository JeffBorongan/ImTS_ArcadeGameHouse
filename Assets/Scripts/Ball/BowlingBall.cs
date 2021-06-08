using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class BowlingBall : MonoBehaviour
{
    private BowlingBallStates currentState = BowlingBallStates.OnGround;
    private XRGrabInteractable interactable;
    private new Collider collider;
    private Rigidbody rigid = null;

    public UnityEvent OnPickedUp = new UnityEvent();
    public UnityEvent OnRelease = new UnityEvent();
    public BoolEvent OnRolling = new BoolEvent();
    public UnityEvent OnSpawn = new UnityEvent();

    public BowlingBallStates CurrentState { get => currentState; }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        interactable = GetComponent<XRGrabInteractable>();
        interactable.selectEntered.AddListener(HandleSelectEntered);
        interactable.selectExited.AddListener(HandleSelectExited);
        OnSpawn.Invoke();
    }

    private void Update()
    {
        if (currentState == BowlingBallStates.OnGround)
        {
            OnRolling.Invoke(rigid.velocity != Vector3.zero);
        }
    }

    private void ChangeState(BowlingBallStates newState)
    {
        currentState = newState;
    }

    private void HandleSelectEntered(SelectEnterEventArgs arg0)
    {
        ChangeState(BowlingBallStates.OnHold);
        OnPickedUp.Invoke();
    }

    private void HandleSelectExited(SelectExitEventArgs arg0)
    {
        ChangeState(BowlingBallStates.OnRelease);
        collider.isTrigger = false;
        OnRelease.Invoke();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnemyAlien")
        {
            gameObject.SetActive(false);
        }

        if (currentState == BowlingBallStates.OnRelease)
        {
            if (other.gameObject.tag == "BowlingLane")
            {
                ChangeState(BowlingBallStates.OnGround);
            }
        }
    }

    private void OnDisable()
    {
        rigid.useGravity = false;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        collider.isTrigger = true;

        OnRelease.RemoveAllListeners();
        OnPickedUp.RemoveAllListeners();
        OnRolling.RemoveAllListeners();
        OnSpawn.RemoveAllListeners();
    }
}

public class BoolEvent : UnityEvent<bool> { }

public enum BowlingBallStates
{
    OnGround,
    OnHold,
    OnRelease
}