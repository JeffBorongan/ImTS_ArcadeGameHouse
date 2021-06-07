using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "BowlingBall")
        {
            gameObject.SetActive(false);
        }
    }
}