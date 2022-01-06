using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRigMimic : MonoBehaviour
{
    public VRMapMimic head;
    public VRMapMimic leftHand;
    public VRMapMimic rightHand;


    private void FixedUpdate()
    {
        head.Map();
        leftHand.Map();
        rightHand.Map();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(leftHand.rigTarget.position, Vector3.one * 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(rightHand.rigTarget.position, Vector3.one * 0.1f);
    }

}

[System.Serializable]
public class VRMapMimic
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public bool inversePosX = false;
    public bool inversePosY = false;
    public bool inversePosZ = false;
    public Quaternion trackingRotationOffset;
    public bool inverseRotX = false;
    public bool inverseRotY = false;
    public bool inverseRotZ = false;

    public void Map()
    {
        Vector3 targetPos = vrTarget.transform.localPosition + trackingPositionOffset;
        targetPos.x = inversePosX ? -targetPos.x : targetPos.x;
        targetPos.y = inversePosY ? -targetPos.y : targetPos.y;
        targetPos.z = inversePosZ ? -targetPos.z : targetPos.z;

        rigTarget.transform.localPosition = targetPos;

        Quaternion targetRot = vrTarget.transform.localRotation * trackingRotationOffset;
        targetRot.x = inverseRotX ? -targetRot.x : targetRot.x;
        targetRot.y = inverseRotY ? -targetRot.y : targetRot.y;
        targetRot.z = inverseRotZ ? -targetRot.z : targetRot.z;

        rigTarget.transform.localRotation = targetRot;
    }

}
