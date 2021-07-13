using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTarget : MonoBehaviour
{
    public Transform destination = null;

    public void EnterDoor(Transform player)
    {
        ScreenFadeManager.Instance.FadeIn(() =>
        {
            player.position = destination.position;
            ScreenFadeManager.Instance.FadeOut(null);
        });
    }
}
