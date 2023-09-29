using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BallDetector : MonoBehaviour
{
    [SerializeField] private BowlingBallSpawner _ballSpawner;
    [SerializeField] private PinSpawner _pinSpawner;

    private void OnTriggerEnter(Collider other)
    {
        XRGrabInteractable ball = other.gameObject.GetComponent<XRGrabInteractable>();

        if (ball != null && !ball.isSelected)
        {
            Debug.Log("ball Collided");
            Destroy(other.gameObject, 10f);
            StartCoroutine(SpawnAfterXSeconds(5f));
        }

        if (other.name.Contains("rings"))
        {
            _pinSpawner.RemovePins();
            Destroy(other.transform.parent.gameObject, 3f);
        }
    }

    private IEnumerator SpawnAfterXSeconds(float seconds)
    {
        yield return new WaitForSeconds(5f);
        _ballSpawner.Spawn();
    }
}
