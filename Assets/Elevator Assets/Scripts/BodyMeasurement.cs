using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMeasurement : MonoBehaviour
{
    public static BodyMeasurement Instance { private set; get; }

    public Transform vrCameraPoint = null;
    public Transform vrLeftHandPoint = null;
    public Transform vrRightHandPoint = null;
    public CharacterCustomization character = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}