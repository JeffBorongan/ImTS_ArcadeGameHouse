using UnityEngine;

public class HeadDetection : MonoBehaviour
{
    #region Parameters

    [SerializeField] private GameObject elevatorTherapistView;
    [SerializeField] private GameObject spaceLobbyTherapistView;
    [SerializeField] private GameObject bowlingGameTherapistView;
    [SerializeField] private GameObject lockEmUpTherapistView;

    #endregion

    #region Trigger Detections

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InsideElevator"))
        {
            elevatorTherapistView.SetActive(true);
            spaceLobbyTherapistView.SetActive(false);
            bowlingGameTherapistView.SetActive(false);
            lockEmUpTherapistView.SetActive(false);
        }

        if (other.CompareTag("InsideSpaceLobby"))
        {
            spaceLobbyTherapistView.SetActive(true);
            elevatorTherapistView.SetActive(false);
            bowlingGameTherapistView.SetActive(false);
            lockEmUpTherapistView.SetActive(false);
            SpaceLobbyManager.Instance.IsInsideSpaceLobby = true;
        }

        if (other.CompareTag("InsideBowlingGame"))
        {
            bowlingGameTherapistView.SetActive(true);
            spaceLobbyTherapistView.SetActive(false);
            elevatorTherapistView.SetActive(false);
            lockEmUpTherapistView.SetActive(false);
        }

        if (other.CompareTag("InsideLockEmUp"))
        {
            lockEmUpTherapistView.SetActive(true);
            bowlingGameTherapistView.SetActive(false);
            spaceLobbyTherapistView.SetActive(false);
            elevatorTherapistView.SetActive(false);
        }
    }

    #endregion
}