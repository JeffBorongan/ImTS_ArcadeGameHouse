using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomAreaDectection : MonoBehaviour
{
    [SerializeField] private EnvironmentPoints pointID = EnvironmentPoints.AvatarRoomCenter;

    [SerializeField] private UnityEvent OnPlayerEnter = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UserInteraction.Instance.SetCurrentPoint(pointID);
            OnPlayerEnter.Invoke();
        }
    }
}
