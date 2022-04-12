using System.Collections;
using UnityEngine;

public class InventoryRoomManager : MonoBehaviour
{
    #region Singleton

    public static InventoryRoomManager Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    #region Parameters

    [SerializeField] private Collider playerDetection = null;

    #endregion

    #region Update

    private void Update()
    {
        playerDetection.enabled = ElevatorManager.Instance.PlayerDetection;
    }

    #endregion

    #region Player Detection

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            CharacterManager.Instance.PointersVisibility(false);
            ElevatorManager.Instance.CloseElevatorDoor();
            StartCoroutine(TeleportCour(5f, (int)Floors.Game3));
        }
    }

    #endregion

    #region Teleport

    private IEnumerator TeleportCour(float waitTime, int floor)
    {
        yield return new WaitForSeconds(waitTime);
        ScreenFadeManager.Instance.FadeIn(() =>
        {
            SceneManager.Instance.LoadFloor((Floors)floor, () =>
            {
                foreach (var item in ElevatorManager.Instance.DisableObjects)
                {
                    item.SetActive(false);
                }

                ScreenFadeManager.Instance.FadeOut(() => { });
            });
        });
    }

    #endregion
}