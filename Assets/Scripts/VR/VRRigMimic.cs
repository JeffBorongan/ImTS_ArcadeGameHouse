using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRigMimic : MonoBehaviour
{
    public Transform headConstaint;
    public Vector3 bodyOffset = Vector3.zero;
    private Vector3 headBodyOffset;

    [SerializeField] private Color headColorGizmos = Color.black;
    public VRMapMimic head;
    [SerializeField] private Color leftHandColorGizmos = Color.black;
    public VRMapMimic leftHand;
    [SerializeField] private Color rightHandColorGizmos = Color.black;
    public VRMapMimic rightHand;

    private void Start()
    {
        headBodyOffset = transform.position - headConstaint.position;
    }

    private void FixedUpdate()
    {
        transform.position = (headConstaint.position + headBodyOffset) + bodyOffset;
        transform.forward = Vector3.ProjectOnPlane(headConstaint.up, Vector3.up).normalized;
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
        Gizmos.DrawSphere(head.GetRigPosition(), 0.1f);
        Gizmos.color = leftHandColorGizmos;
        Gizmos.DrawSphere(leftHand.GetRigPosition(), 0.1f);
        Gizmos.color = rightHandColorGizmos;
        Gizmos.DrawSphere(rightHand.GetRigPosition(), .1f);
    }
}

[System.Serializable]
public class VRMapMimic
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingGlobalPositionOffset;
    public Vector3 trackingRotationOffset;
    public bool inversePosition = false;
    public bool inverseRotation = false;

    public void Map()
    {
        rigTarget.position = trackingGlobalPositionOffset + (inversePosition ? vrTarget.TransformPoint(trackingPositionOffset) :  vrTarget.TransformPoint(trackingPositionOffset));
        rigTarget.rotation = inversePosition ? Quaternion.Inverse(vrTarget.rotation * Quaternion.Euler(trackingRotationOffset)) : vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }

    public Vector3 GetRigPosition()
    {
        return trackingGlobalPositionOffset + (inversePosition ? vrTarget.TransformPoint(trackingPositionOffset) : vrTarget.TransformPoint(trackingPositionOffset));
    }

}
