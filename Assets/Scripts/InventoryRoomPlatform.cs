using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryRoomPlatform : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            StartCoroutine(TeleportCour(5f, 5));
        }
    }

    IEnumerator TeleportCour(float waitTime, int floor)
    {
        yield return new WaitForSeconds(waitTime);
        ScreenFadeManager.Instance.FadeIn(() =>
        {
            SceneManager.Instance.LoadFloor((Floors)floor, () =>
            {
                ElevatorManager.Instance.ElevatorPrefab.SetActive(false);
                ScreenFadeManager.Instance.FadeOut(() =>
                {

                });
            });
        });
    }
}