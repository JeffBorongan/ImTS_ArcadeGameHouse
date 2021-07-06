using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    public List<BodyPart> bodyParts = new List<BodyPart>();
    private Dictionary<BodyPartID, BodyPart> bodyPartDictionary = new Dictionary<BodyPartID, BodyPart>();

    private void Start()
    {
        foreach (var bodyPart in bodyParts)
        {
            bodyPartDictionary.Add(bodyPart.id, bodyPart);
        }
    }


    public void ChangeBodyPart(bool next, BodyPartID id, UIBodyPart ui)
    {
        bodyPartDictionary[id].ChangeMaterial(next, ui);
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
        if (next)
        {
            if(currentMaterialAttached + 1 < bodyPartMaterials.Count)
            {
                currentMaterialAttached++;

                foreach (var bodyPartRender in bodyPartsRenderer)
                {
                    bodyPartRender.material = bodyPartMaterials[currentMaterialAttached];
                    ui.txtLabel.color = bodyPartMaterials[currentMaterialAttached].color;
                }
            }
        }
        else
        {
            if(currentMaterialAttached - 1 <= 0)
            {
                currentMaterialAttached--;

                foreach (var bodyPartRender in bodyPartsRenderer)
                {
                    bodyPartRender.material = bodyPartMaterials[currentMaterialAttached];
                    ui.txtLabel.color = bodyPartMaterials[currentMaterialAttached].color;
                }
            }
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