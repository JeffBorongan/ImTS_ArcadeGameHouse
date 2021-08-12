using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAreaDectection : MonoBehaviour
{
    [SerializeField] private EnvironmentPoints pointID = EnvironmentPoints.AvatarRoomCenter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UserInteraction.Instance.SetCurrentPoint(pointID);
        }
    }
}
