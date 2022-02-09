using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryList : MonoBehaviour
{
    [SerializeField] private GameObject[] accessories;

    public void ChangeCurrentCostume(GameObject enableCostume, GameObject disableCostume)
    {
        enableCostume.SetActive(true);
        disableCostume.SetActive(false);
    }
}