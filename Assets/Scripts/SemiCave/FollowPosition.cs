using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    [SerializeField] private GameObject trackedObject;
    [SerializeField] private bool trackX;
    [SerializeField] private float xOffset;

    [SerializeField] private bool trackY;
    [SerializeField] private float yOffset;

    [SerializeField] private bool trackZ;
    [SerializeField] private float zOffset;

    float x, y, z;

    void Update()
    {
        if (trackX) x = trackedObject.transform.localPosition.x;
        else x = transform.localPosition.x;
        if (trackY) y = trackedObject.transform.localPosition.y;
        else y = transform.localPosition.y;
        if (trackZ) z = trackedObject.transform.localPosition.z;
        else z = transform.localPosition.z;

        x += xOffset;
        y += yOffset;
        z += zOffset;

        transform.localPosition = new Vector3(x,y,z);
    }
}
