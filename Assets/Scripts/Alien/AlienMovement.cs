using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AlienMovement : MonoBehaviour
{
    public UnityEvent OnDeath = new UnityEvent();
    public UnityEvent OnReachDestination = new UnityEvent();
    public UnityEvent OnWalking = new UnityEvent();
    public UnityEvent OnSpawn = new UnityEvent();

    public Transform pathPoint;
    [SerializeField] private NavMeshAgent alienAgent;
    Vector3 targetDestination = Vector3.zero;


    private void Update()
    {
        float dist = alienAgent.remainingDistance; 
        if (dist != Mathf.Infinity && alienAgent.pathStatus == NavMeshPathStatus.PathComplete && alienAgent.remainingDistance == 0)
        {
            OnReachDestination.Invoke();
            gameObject.SetActive(false);
        }

        if (alienAgent.remainingDistance > 0)
        {
            OnWalking.Invoke();
        }
    }

    public void SetMovementSpeed(float newSpeed)
    {
        alienAgent.speed = newSpeed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "BowlingBall")
        {
            OnDeath.Invoke();
            gameObject.SetActive(false);
        }
    }

    void GoToTheCockpit()
    {
        targetDestination = new Vector3(transform.position.x, pathPoint.position.y, pathPoint.position.z);
        alienAgent.SetDestination(targetDestination);
    }

    private void OnEnable()
    {
        OnSpawn.Invoke();
        GoToTheCockpit();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetDestination, 1f);
    }
}