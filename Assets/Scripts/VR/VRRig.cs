using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRig : MonoBehaviour
{
    public Transform headConstaint;
    public Vector3 headBodyOffset;

    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    private void Start()
    {
        headBodyOffset = transform.position - headConstaint.position;
    }

    private void FixedUpdate()
    {
        transform.position = headConstaint.position + headBodyOffset;
        transform.forward = Vector3.ProjectOnPlane(headConstaint.up, Vector3.up).normalized;
        head.Map();
        leftHand.Map();
        rightHand.Map();
    }

    public void ResetHeadBodyOffset()
    {
        headBodyOffset = transform.position - headConstaint.position;
    }
}

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }

}
