using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationTemporary : MonoBehaviour
{
    public void EnablePart(int enable)
    {
        BodyMeasurement.Instance.customizePart[enable].SetActive(true);
    }

    public void DisablePart(int disable)
    {
        BodyMeasurement.Instance.customizePart[disable].SetActive(false);
    }
}