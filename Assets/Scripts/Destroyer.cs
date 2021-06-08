using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BowlingBall")
        {
            other.gameObject.SetActive(false);
        }
    }
}