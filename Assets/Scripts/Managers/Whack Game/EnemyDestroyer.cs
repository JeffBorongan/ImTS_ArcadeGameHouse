using UnityEngine;

public class EnemyDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAlien"))
        {
            other.GetComponent<AlienMovement>().OnReachDestination.Invoke();
        }
    }
}