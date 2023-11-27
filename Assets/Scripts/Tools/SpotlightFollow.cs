using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpotlightFollow : MonoBehaviour
{
    [SerializeField] private Transform _spotlightFollow;

    private void Start()
    {
        Debug.Log("SpotlightFollow Editor Script Running");
    }

    private void Update()
    {
        if (_spotlightFollow != null)
        { 
            transform.LookAt(_spotlightFollow.position);
        }
    }

    public void SetFollowTransform(Transform followTransform)
    {
        if(_spotlightFollow == null)
        {
            _spotlightFollow = followTransform;
        }
    }
}
