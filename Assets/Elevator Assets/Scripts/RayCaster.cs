using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    public float castDistance = 10f;
    public bool isCastOnHit;
    public LayerMask detectionMask;

    RaycastHit castOnHit;

    private void Update()
    {
        rayCast();
    }

    public void rayCast()
    {
        
            isCastOnHit = Physics.BoxCast(transform.position, transform.lossyScale / 2, transform.right,
                                          out castOnHit, transform.rotation, castDistance, detectionMask);
        if (isCastOnHit)
        {
            //ElevatorFloorManager.Instance.isDoorBlocked = true;
        }
        else
        {
            //if (ElevatorFloorManager.Instance.isDoorBlocked)
            //{
            //    ElevatorFloorManager.Instance.isDoorBlocked = false;
            //    ElevatorFloorManager.Instance.closeElevatorDoor();
            //}
        }
        
    }

    private void OnDrawGizmos()
    {
        if (isCastOnHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.right * castOnHit.distance);
            Gizmos.DrawWireCube(transform.position + transform.right * castOnHit.distance, transform.lossyScale);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.right * castDistance);
        }
    }
}
