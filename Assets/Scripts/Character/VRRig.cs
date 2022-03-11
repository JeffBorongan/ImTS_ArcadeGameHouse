using UnityEngine;

public class VRRig : MonoBehaviour
{
    #region Parameters

    [SerializeField] private Transform headConstraint = null;
    [SerializeField] private Vector3 bodyOffset = Vector3.zero;
    [SerializeField] private VRMap head = new VRMap();
    [SerializeField] private VRMap leftHand = new VRMap();
    [SerializeField] private VRMap rightHand = new VRMap();
    private Vector3 headBodyOffset = Vector3.zero;
    private bool isStationary = false;

    #endregion

    #region Encapsulations

    public VRMap Head { get => head; }
    public bool IsStationary { get => isStationary; set => isStationary = value; }

    #endregion

    #region Functions

    private void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
    }

    private void FixedUpdate()
    {
        transform.position = (headConstraint.position + headBodyOffset) + bodyOffset;

        if (!IsStationary)
        {
            transform.forward = Vector3.ProjectOnPlane(headConstraint.forward, Vector3.up).normalized;
        }

        Head.Map();
        leftHand.Map();
        rightHand.Map();
    }

    public void ResetHeadBodyOffset()
    {
        headBodyOffset = transform.position - headConstraint.position;
    }

    #endregion
}

[System.Serializable]
public class VRMap
{
    #region Parameters

    [SerializeField] private Transform vRTarget = null;
    [SerializeField] private Transform rigTarget = null;
    [SerializeField] private Vector3 trackingPositionOffset = Vector3.zero;
    [SerializeField] private Vector3 trackingRotationOffset = Vector3.zero;

    #endregion

    #region Encapsulations

    public Transform VRTarget { get => vRTarget; }

    #endregion

    #region Functions

    public void Map()
    {
        rigTarget.position = VRTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = VRTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }

    #endregion
}