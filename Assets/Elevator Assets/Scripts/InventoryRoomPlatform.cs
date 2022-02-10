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
            SceneManagement.Instance.LoadFloor((floor)floor, () =>
            {
                ElevatorFloorManager.Instance.elevatorPrefab.SetActive(false);
                ScreenFadeManager.Instance.FadeOut(() =>
                {

                });
            });
        });
    }
}