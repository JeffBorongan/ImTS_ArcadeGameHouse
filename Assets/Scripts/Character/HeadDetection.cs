using UnityEngine;

public class HeadDetection : MonoBehaviour
{
    #region Parameters

    [SerializeField] private GameObject elevatorTherapistView = null;
    [SerializeField] private GameObject spaceLobbyTherapistView = null;
    [SerializeField] private GameObject bowlingGameTherapistView = null;
    [SerializeField] private GameObject lockEmUpTherapistView = null;
    [SerializeField] private GameObject inventoryRoomTherapistView = null;
    [SerializeField] private GameObject walkeyMoleyTherapistView = null;
    [SerializeField] private GameObject outsideElevatorOpenButton = null;
    [SerializeField] private GameObject outsideElevatorCloseButton = null;

    #endregion

    #region Trigger Detections

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InsideElevator"))
        {
            UsabilityHelper.Instance.StopUsabilityTimer(UsabilityTimer.Bowling);
            UsabilityHelper.Instance.StopUsabilityTimer(UsabilityTimer.Lock);
            UsabilityHelper.Instance.StopUsabilityTimer(UsabilityTimer.Walkey);

            elevatorTherapistView.SetActive(true);
            spaceLobbyTherapistView.SetActive(false);
            bowlingGameTherapistView.SetActive(false);
            lockEmUpTherapistView.SetActive(false);
            inventoryRoomTherapistView.SetActive(false);
            walkeyMoleyTherapistView.SetActive(false);

            outsideElevatorOpenButton.SetActive(false);
            outsideElevatorCloseButton.SetActive(false);
        }

        if (other.CompareTag("InsideSpaceLobby"))
        {
            spaceLobbyTherapistView.SetActive(true);
            elevatorTherapistView.SetActive(false);
            bowlingGameTherapistView.SetActive(false);
            lockEmUpTherapistView.SetActive(false);
            inventoryRoomTherapistView.SetActive(false);
            walkeyMoleyTherapistView.SetActive(false);

            outsideElevatorOpenButton.SetActive(false);
            outsideElevatorCloseButton.SetActive(false);

            SpaceLobbyManager.Instance.IsInsideSpaceLobby = true;
        }

        if (other.CompareTag("InsideBowlingGame"))
        {
            UsabilityHelper.Instance.StopUsabilityTimer(UsabilityTimer.Elevator);
            UsabilityHelper.Instance.StartUsabilityTimer(UsabilityTimer.Bowling);

            bowlingGameTherapistView.SetActive(true);
            spaceLobbyTherapistView.SetActive(false);
            elevatorTherapistView.SetActive(false);
            lockEmUpTherapistView.SetActive(false);
            inventoryRoomTherapistView.SetActive(false);
            walkeyMoleyTherapistView.SetActive(false);

            outsideElevatorOpenButton.SetActive(true);
            outsideElevatorCloseButton.SetActive(true);
        }

        if (other.CompareTag("InsideLockEmUp"))
        {
            UsabilityHelper.Instance.StartUsabilityTimer(UsabilityTimer.Lock);

            lockEmUpTherapistView.SetActive(true);
            bowlingGameTherapistView.SetActive(false);
            spaceLobbyTherapistView.SetActive(false);
            elevatorTherapistView.SetActive(false);
            inventoryRoomTherapistView.SetActive(false);
            walkeyMoleyTherapistView.SetActive(false);
            outsideElevatorOpenButton.SetActive(true);
            outsideElevatorCloseButton.SetActive(true);
        }

        if (other.CompareTag("InsideInventoryRoom"))
        {
            UsabilityHelper.Instance.StartUsabilityTimer(UsabilityTimer.Walkey);

            inventoryRoomTherapistView.SetActive(true);
            lockEmUpTherapistView.SetActive(false);
            bowlingGameTherapistView.SetActive(false);
            spaceLobbyTherapistView.SetActive(false);
            elevatorTherapistView.SetActive(false);
            walkeyMoleyTherapistView.SetActive(false);

            outsideElevatorOpenButton.SetActive(true);
            outsideElevatorCloseButton.SetActive(true);
        }

        if (other.CompareTag("InsideWalkeyMoley"))
        {
            walkeyMoleyTherapistView.SetActive(true);
            inventoryRoomTherapistView.SetActive(false);
            lockEmUpTherapistView.SetActive(false);
            bowlingGameTherapistView.SetActive(false);
            spaceLobbyTherapistView.SetActive(false);
            elevatorTherapistView.SetActive(false);

            outsideElevatorOpenButton.SetActive(false);
            outsideElevatorCloseButton.SetActive(false);
        }
    }

    #endregion
}