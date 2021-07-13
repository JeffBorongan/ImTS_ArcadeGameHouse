using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    [SerializeField] private float maxHeight = 1f;
    public List<BodyPart> bodyParts = new List<BodyPart>();
    private Dictionary<BodyPartID, BodyPart> bodyPartDictionary = new Dictionary<BodyPartID, BodyPart>();
    private VRRig rig = null;

    private void Start()
    {
        rig = GetComponent<VRRig>();

        foreach (var bodyPart in bodyParts)
        {
            bodyPartDictionary.Add(bodyPart.id, bodyPart);
            bodyPart.ChangeMaterial(bodyPart.bodyPartMaterials[0]);
        }
    }

    public void SetHeight()
    {
        float cameraHeight = rig.head.vrTarget.localPosition.y;
        float scaleFactor = cameraHeight / maxHeight;
        transform.localScale = (Vector3.one / 2) * scaleFactor;

        rig.ResetHeadBodyOffset();
    }
}

[System.Serializable]
public class BodyPart
{
    public BodyPartID id = BodyPartID.HELMET;
    public List<MeshRenderer> bodyPartsRenderer = new List<MeshRenderer>();
    public List<Material> bodyPartMaterials = new List<Material>();

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
    BODYSUIT,
    JETPACK,
    SPACEBOOTS,
    GLOVES,
    WRISTGRADGET,
    Count
}