using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AlienMovement : MonoBehaviour
{
    public UnityEvent OnDeath = new UnityEvent();
    public UnityEvent OnReachDestination = new UnityEvent();
    public Transform pathPoint;
    private NavMeshAgent alienAgent;
    private float currentSpeed = 0f;

    private void Start()
    {
        alienAgent = GetComponent<NavMeshAgent>();
        Vector3 spawnPosition = new Vector3(gameObject.transform.position.x, pathPoint.position.y, pathPoint.position.z);
        alienAgent.SetDestination(spawnPosition);
        alienAgent.speed = currentSpeed;
    }

    private void Update()
    {
        if (alienAgent.remainingDistance <= 0.5)
        {
            OnReachDestination.Invoke();
        }
    }

    public void SetMovementSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "BowlingBall")
        {
            OnDeath.Invoke();
            Destroy(gameObject);
        }
    }
}