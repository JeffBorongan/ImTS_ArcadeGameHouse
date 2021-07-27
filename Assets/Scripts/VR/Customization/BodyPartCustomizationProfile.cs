using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Body Part Profile", menuName = "Customization/Body Part", order = 0)]
public class BodyPartCustomizationProfile : ScriptableObject
{
    public Sprite bodyPartSpriteUI = null;
    public Material bodyPartMaterial = null;
    public int starCost = 0;
}
