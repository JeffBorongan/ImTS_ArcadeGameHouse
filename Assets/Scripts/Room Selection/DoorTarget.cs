using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTarget : MonoBehaviour
{
    [SerializeField] private string roomName = "";
    public Transform destination = null;

    public string RoomName { get => roomName; }

    public void EnterDoor(Transform player)
    {
        ScreenFadeManager.Instance.FadeIn(() =>
        {
            player.position = destination.position;
            ScreenFadeManager.Instance.FadeOut(null);
        });
    }
}
