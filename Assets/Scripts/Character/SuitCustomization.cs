using System.Collections.Generic;
using UnityEngine;

public class SuitCustomization : MonoBehaviour
{
    #region Parameters

    [SerializeField] private VRRig rig = null;
    [SerializeField] private float maxHeight = 1f;
    [SerializeField] private List<GameObject> suitParts = new List<GameObject>();

    #endregion

    #region Encapsulations

    public List<GameObject> SuitParts { get => suitParts; }

    #endregion

    #region Suit Height

    public void SetHeight()
    {
        float cameraHeight = rig.Head.VRTarget.localPosition.y;
        float scaleFactor = cameraHeight / maxHeight;
        transform.localScale = Vector3.one * scaleFactor;
        rig.ResetHeadBodyOffset();
    }

    #endregion
}