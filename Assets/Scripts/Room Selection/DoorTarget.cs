using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTarget : MonoBehaviour
{
    [SerializeField] private string roomName = "";
    [SerializeField] private EnvironmentPoints point = EnvironmentPoints.AvatarRoomCenter;
    [SerializeField] private UnityEvent OnEnterRoom = new UnityEvent();
    public Transform destination = null;

    public string RoomName { get => roomName; }
    public EnvironmentPoints Point { get => point; }

    public void EnterDoor(Transform player)
    {
        ScreenFadeManager.Instance.FadeIn(() =>
        {
            player.position = destination.position;
            ScreenFadeManager.Instance.FadeOut(() => 
            {
                if(OnEnterRoom != null)
                {
                    OnEnterRoom.Invoke();
                }
            });
        });
    }
}
