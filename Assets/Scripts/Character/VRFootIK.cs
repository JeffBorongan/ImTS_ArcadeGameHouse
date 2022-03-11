using UnityEngine;
using UnityEngine.Animations.Rigging;

public class VRFootIK : MonoBehaviour
{
    #region Parameters

    [SerializeField] private LayerMask footIKMask;
    [SerializeField] private Vector3 footOffset = Vector3.zero;
    [Range(0, 1)]
    [SerializeField] private float rightFootPosWeight = 1;
    [Range(0, 1)]
    [SerializeField] private float rightFootRotWeight = 1;
    [Range(0, 1)]
    [SerializeField] private float leftFootPosWeight = 1;
    [Range(0, 1)]
    [SerializeField] private float leftFootRotWeight = 1;
    [SerializeField] private TwoBoneIKConstraint leftLegConstraint = null;
    [SerializeField] private TwoBoneIKConstraint rightLegConstraint = null;
    [SerializeField] private bool footPlacedOnBox = false;
    private Animator animator = null;
    private int legSelected = 0;

    #endregion

    #region Functions

    private void Start()
    {
        animator = GetComponent<Animator>();
        legSelected = AnatomyCaptureManager.Instance.LegSelected;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(footPlacedOnBox) { return; }

        Vector3 rightFootPos = animator.GetIKPosition(AvatarIKGoal.RightFoot);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(rightFootPos + Vector3.up, Vector3.down, out hit, Mathf.Infinity, footIKMask);

        if (hasHit)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootPosWeight);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, hit.point + footOffset);
            Quaternion rightFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootRotWeight);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
        }

        Vector3 leftFootPos = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        hasHit = Physics.Raycast(leftFootPos + Vector3.up, Vector3.down, out hit, Mathf.Infinity, footIKMask);

        if (hasHit)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootPosWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, hit.point + footOffset);
            Quaternion leftFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootRotWeight);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
        }
    }

    public void PlaceLegOnBox()
    {
        UnPlaceAllLegOnBox();
        footPlacedOnBox = true;

        if (legSelected == 0)
        {
            leftLegConstraint.weight = 1;
        }
        else
        {
            rightLegConstraint.weight = 1;
        }
    }

    public void UnPlaceAllLegOnBox()
    {
        leftLegConstraint.weight = 0;
        rightLegConstraint.weight = 0;
        footPlacedOnBox = false;
    }

    #endregion
}