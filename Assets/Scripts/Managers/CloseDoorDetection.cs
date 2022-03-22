using UnityEngine;

public class CloseDoorDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            ElevatorManager.Instance.CloseElevatorDoor();
            CharacterManager.Instance.PointersVisibility(false);
            gameObject.SetActive(false);
        }
    }
}