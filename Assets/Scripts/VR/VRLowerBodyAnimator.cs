using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRLowerBodyAnimator : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float smoothing = 1f;
    [SerializeField] private Transform head = null;
    private Animator animator = null;
    private Vector3 previousPos = Vector3.zero;

    private void Start()
    {
        animator = GetComponent<Animator>();
        previousPos = head.position;
    }

    private void Update()
    {
        Vector3 headsetSpeed = (head.position - previousPos) / Time.deltaTime;
        headsetSpeed.y = 0f;

        Vector3 headsetLocalSpeed = transform.InverseTransformDirection(headsetSpeed);
        previousPos = head.position;

        float previousBlendX = animator.GetFloat("BlendX");
        float previousBlendY = animator.GetFloat("BlendY");

        animator.SetFloat("BlendX", Mathf.Lerp(previousBlendX, Mathf.Clamp(headsetLocalSpeed.x, -1f, 1f), smoothing));
        animator.SetFloat("BlendY", Mathf.Lerp(previousBlendY, Mathf.Clamp(headsetLocalSpeed.z, -1f, 1f), smoothing));
    }


}
