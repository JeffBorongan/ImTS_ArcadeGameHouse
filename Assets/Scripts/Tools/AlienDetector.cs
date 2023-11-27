using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AlienDetector : MonoBehaviour
{
    [SerializeField] private SpotlightFollow _spotlightFollow;

    private void Start()
    {
        Debug.Log("AlienDetector Editor Script Running");
        _spotlightFollow = GetComponentInChildren<SpotlightFollow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AlienMovement>())
        {
            _spotlightFollow.SetFollowTransform(other.transform);
        }
    }
}
