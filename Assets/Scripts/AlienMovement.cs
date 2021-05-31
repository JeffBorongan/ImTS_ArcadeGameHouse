using UnityEngine;
using UnityEngine.AI;

public class AlienMovement : MonoBehaviour
{
    public Transform pathPoint;
    private NavMeshAgent alienAgent;

    private void Start()
    {
        alienAgent = GetComponent<NavMeshAgent>();
        Vector3 spawnPosition = new Vector3(gameObject.transform.position.x, pathPoint.position.y, pathPoint.position.z);
        alienAgent.SetDestination(spawnPosition);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "BowlingBall")
        {
            Destroy(gameObject);
        }
    }
}