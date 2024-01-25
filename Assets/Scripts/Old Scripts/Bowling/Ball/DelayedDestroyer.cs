using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DelayedDestroyer : MonoBehaviour
{
    [SerializeField] private float _delay = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BowlingBall")
        {
            StartCoroutine(HideAfterXSeconds(other.gameObject));
        }
    }

    private IEnumerator HideAfterXSeconds(GameObject ball)
    {
        yield return new WaitForSeconds(_delay);
        ball.SetActive(false);
    }
}