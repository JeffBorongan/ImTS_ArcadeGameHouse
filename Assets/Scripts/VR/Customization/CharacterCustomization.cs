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
        }
    }


    public void ChangeBodyPart(bool next, BodyPartID id, UIBodyPart ui)
    {
        bodyPartDictionary[id].ChangeMaterial(next, ui);
    }

    public void SetHeight()
    {
        float cameraHeight = rig.head.vrTarget.localPosition.y;
        float scaleFactor = cameraHeight / maxHeight;
        transform.localScale = Vector3.one * scaleFactor;
    }
}

[System.Serializable]
public class BodyPart
{
    public BodyPartID id = BodyPartID.Head;
    public List<MeshRenderer> bodyPartsRenderer = new List<MeshRenderer>();
    public List<Material> bodyPartMaterials = new List<Material>();
    public int currentMaterialAttached = 0;

    public void ChangeMaterial(bool next, UIBodyPart ui)
    {
        currentMaterialAttached = next ? (currentMaterialAttached + 1 < bodyPartMaterials.Count ? currentMaterialAttached + 1 : 0) : (currentMaterialAttached - 1 >= 0 ? currentMaterialAttached - 1 : bodyPartMaterials.Count - 1);

        foreach (var bodyPartRender in bodyPartsRenderer)
        {
            bodyPartRender.material = bodyPartMaterials[currentMaterialAttached];
            ui.txtLabel.color = bodyPartMaterials[currentMaterialAttached].color;
        }
    }
}

public enum BodyPartID
{
    Head,
    Arms,
    Chest,
    Legs
}