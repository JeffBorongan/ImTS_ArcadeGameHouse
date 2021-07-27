using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    [SerializeField] private float maxHeight = 1f;
    public List<BodyPart> bodyParts = new List<BodyPart>();
    [SerializeField] private VRRig rig = null;

    public void SetHeight()
    {
        float cameraHeight = rig.head.vrTarget.localPosition.y;
        float scaleFactor = cameraHeight / maxHeight;
        transform.localScale = (Vector3.one * 0.8f) * scaleFactor;

        rig.ResetHeadBodyOffset();
    }
}

[System.Serializable]
public class BodyPart
{
    public BodyPartID id = BodyPartID.HELMET;
    public List<SkinnedMeshRenderer> bodyPartsRenderer = new List<SkinnedMeshRenderer>();

    public void ChangeMaterial(Material material)
    {
        foreach (var bodyPartRender in bodyPartsRenderer)
        {
            bodyPartRender.material = material;
        }
    }
}

public enum BodyPartID
{
    HELMET,
    SUIT,
    VEST,
    GLOVES,
    WRISTBAND,
    JOINTPADS,
    JETPACK,
    BOOTS,
    Count
}