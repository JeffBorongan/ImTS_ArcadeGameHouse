using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlienMovement : MonoBehaviour
{
    public List<Transform> pathPoints = new List<Transform>();
    private NavMeshAgent alienAgent;

    private void Start()
    {
        alienAgent = GetComponent<NavMeshAgent>();
        alienAgent.SetDestination(pathPoints[0].position);
    }
}