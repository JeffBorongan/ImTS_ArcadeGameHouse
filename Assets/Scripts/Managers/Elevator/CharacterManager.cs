using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    #region Singleton

    public static CharacterManager Instance { private set; get; }

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

    #endregion

    #region Parameters

    [SerializeField] private Transform vRCamera = null;
    [SerializeField] private Transform vRLeftHand = null;
    [SerializeField] private Transform vRRightHand = null;
    [SerializeField] private VRRig vRRig = null;
    [SerializeField] private VRFootIK vRFootIK = null;
    [SerializeField] private SuitCustomization suitCustomization = null;
    [SerializeField] private Dictionary<string, Vector3> currentAnatomy = new Dictionary<string, Vector3>();
    [SerializeField] private Transform playerLocation = null;
    [SerializeField] private GameObject characterPrefab = null;
    [SerializeField] private GameObject characterSuit = null;
    [SerializeField] private GameObject leftHandPointer = null;
    [SerializeField] private GameObject rightHandPointer = null;

    #endregion

    #region Encapsulations

    public Transform VRCamera { get => vRCamera; }
    public Transform VRLeftHand { get => vRLeftHand; }
    public Transform VRRightHand { get => vRRightHand; }
    public VRRig VRRig { get => vRRig; }
    public VRFootIK VRFootIK { get => vRFootIK; }
    public SuitCustomization SuitCustomization { get => suitCustomization; }
    public Dictionary<string, Vector3> CurrentAnatomy { get => currentAnatomy; set => currentAnatomy = value; }
    public Transform PlayerLocation { get => playerLocation; }
    public GameObject CharacterPrefab { get => characterPrefab; }
    public GameObject CharacterSuit { get => characterSuit; }

    #endregion

    #region Pointers Visibility

    public void PointersVisibility(bool visible)
    {
        leftHandPointer.SetActive(visible);
        rightHandPointer.SetActive(visible);
    }

    #endregion
}