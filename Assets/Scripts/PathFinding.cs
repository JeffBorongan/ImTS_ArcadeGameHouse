using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinding : MonoBehaviour
{
    [SerializeField] private Transform destination = null;
    private NavMeshAgent agent = null;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(destination.position);
    }
}
