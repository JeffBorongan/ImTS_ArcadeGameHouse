using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMeasurement : MonoBehaviour
{
    public static BodyMeasurement Instance { private set; get; }

    [SerializeField] private Transform vrCameraPoint = null;
    [SerializeField] private Transform vrLeftHandPoint = null;
    [SerializeField] private Transform vrRightHandPoint = null;
    [SerializeField] private CharacterCustomization characterCustomization = null;
    [SerializeField] private VRRig vrRig = null;
    [SerializeField] private VRFootIK vrFootIK = null;
    [SerializeField] private Dictionary<string, Vector3> currentAnatomy = new Dictionary<string, Vector3>();

    public GameObject[] customizePart = null;

    public Transform VrCameraPoint { get => vrCameraPoint; }
    public Transform VrLeftHandPoint { get => vrLeftHandPoint; }
    public Transform VrRightHandPoint { get => vrRightHandPoint; }
    public CharacterCustomization CharacterCustomization { get => characterCustomization; }
    public VRRig VrRig { get => vrRig; }
    public VRFootIK VrFootIK { get => vrFootIK; }
    public Dictionary<string, Vector3> CurrentAnatomy { get => currentAnatomy; set => currentAnatomy = value; }

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