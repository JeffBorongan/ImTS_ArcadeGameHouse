using System.Collections.Generic;
using UnityEngine;

public class SuitCustomization : MonoBehaviour
{
    [SerializeField] private VRRig rig = null;
    [SerializeField] private float maxHeight = 1f;
    [SerializeField] private List<GameObject> suitParts = new List<GameObject>();

    public List<GameObject> SuitParts { get => suitParts; }

    public void SetHeight()
    {
        float cameraHeight = rig.Head.VRTarget.localPosition.y;
        float scaleFactor = cameraHeight / maxHeight;
        transform.localScale = Vector3.one * scaleFactor;
        rig.ResetHeadBodyOffset();
    }
}