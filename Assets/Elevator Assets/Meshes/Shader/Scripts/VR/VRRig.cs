using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRig : MonoBehaviour
{
    public Transform headConstaint;
    public Vector3 bodyOffset = Vector3.zero;
    private Vector3 headBodyOffset;

    [SerializeField] private Color headColorGizmos = Color.black;
    public VRMap head;
    [SerializeField] private Color leftHandColorGizmos = Color.black;
    public VRMap leftHand;
    [SerializeField] private Color rightHandColorGizmos = Color.black;
    public VRMap rightHand;

    public bool IsStationary = false;

    private void Start()
    {
        headBodyOffset = transform.position - headConstaint.position;
    }

    private void FixedUpdate()
    {
        transform.position = (headConstaint.position + headBodyOffset) + bodyOffset;

        if (!IsStationary)
        {
            transform.forward = Vector3.ProjectOnPlane(headConstaint.forward, Vector3.up).normalized;
        }

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }

    public void ResetHeadBodyOffset()
    {
        headBodyOffset = transform.position - headConstaint.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = headColorGizmos;
        Gizmos.DrawSphere(head.vrTarget.TransformPoint(head.trackingPositionOffset), 0.1f);
        Gizmos.color = leftHandColorGizmos;
        Gizmos.DrawSphere(leftHand.vrTarget.TransformPoint(leftHand.trackingPositionOffset), 0.1f);
        Gizmos.color = rightHandColorGizmos;
        Gizmos.DrawSphere(rightHand.vrTarget.TransformPoint(rightHand.trackingPositionOffset), .1f);
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
