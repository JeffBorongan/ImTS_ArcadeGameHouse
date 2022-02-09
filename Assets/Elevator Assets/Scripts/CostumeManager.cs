using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeManager : MonoBehaviour
{
    public List<CostumePart> costumeParts = new List<CostumePart>();
}

[System.Serializable]
public class CostumePart
{
    public CostumePartID id = CostumePartID.HEAD;
    public AccessoryList partReference;
}

public enum CostumePartID
{
    HEAD,
    CHEST,
    LEFT_ARM,
    LEFT_FOREARM,
    LEFT_HAND,
    LEFT_THIGH,
    LEFT_LEG,
    LEFT_FOOT,
    RIGHT_ARM,
    RIGHT_FOREARM,
    RIGHT_HAND,
    RIGHT_THIGH,
    RIGHT_LEG,
    RIGHT_FOOT,
    Count
}