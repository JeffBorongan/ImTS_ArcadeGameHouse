using System.Collections;
using UnityEngine;

public class SpaceLobbyManager : MonoBehaviour
{
    #region Singleton

    public static SpaceLobbyManager Instance { private set; get; }

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

    [SerializeField] private Trophy trophy = null;
    [SerializeField] private GameObject closeDoorDetection = null;
    [SerializeField] private GameObject game1Trophy = null;
    [SerializeField] private GameObject game2Trophy = null;
    [SerializeField] private GameObject game3Trophy = null;
    [SerializeField] private GameObject game1TrophyDisplay = null;
    [SerializeField] private GameObject game2TrophyDisplay = null;
    [SerializeField] private GameObject game3TrophyDisplay = null;
    [SerializeField] private GameObject game1TrophyHologram = null;
    [SerializeField] private GameObject game2TrophyHologram = null;
    [SerializeField] private GameObject game3TrophyHologram = null;
    [SerializeField] private GameObject game1TrophyLight = null;
    [SerializeField] private GameObject game2TrophyLight = null;
    [SerializeField] private GameObject game3TrophyLight = null;
    [SerializeField] private GameObject spaceshipHologram = null;
    [SerializeField] private AudioClip givingTrophyClip = null;
    private GameNumber gameNumber = GameNumber.None;
    private bool isInsideSpaceLobby = false;
    private bool isTrophyGiven = false;
    private bool isGivingTrophy = false;

    #endregion

    #region Encapsulations

    public GameObject Game1Trophy { get => game1Trophy; }
    public GameObject Game2Trophy { get => game2Trophy; }
    public GameObject Game3Trophy { get => game3Trophy; }
    public GameObject Game1TrophyDisplay { get => game1TrophyDisplay; }
    public GameObject Game2TrophyDisplay { get => game2TrophyDisplay; }
    public GameObject Game3TrophyDisplay { get => game3TrophyDisplay; }
    public GameObject Game1TrophyHologram { get => game1TrophyHologram; }
    public GameObject Game2TrophyHologram { get => game2TrophyHologram; }
    public GameObject Game3TrophyHologram { get => game3TrophyHologram; }
    public GameObject Game1TrophyLight { get => game1TrophyLight; }
    public GameObject Game2TrophyLight { get => game2TrophyLight; }
    public GameObject Game3TrophyLight { get => game3TrophyLight; }
    public GameNumber GameNumber { get => gameNumber; set => gameNumber = value; }
    public bool IsInsideSpaceLobby { get => isInsideSpaceLobby; set => isInsideSpaceLobby = value; }

    #endregion

    #region Start

    private void Start()
    {
        closeDoorDetection.SetActive(ElevatorManager.Instance.CloseDoorDetection);
    }

    #endregion

    #region Update

    private void Update()
    {
        spaceshipHologram.transform.Rotate(0, 0, 10 * Time.deltaTime);
        StartCoroutine(TrophyMechanics());
    }

    #endregion

    #region Elevator Functions

    public void OpenElevatorDoor()
    {
        ElevatorManager.Instance.OpenElevatorDoor();
    }

    public void GetElevatorPart(MeshRenderer meshRenderer)
    {
        ElevatorManager.Instance.GetElevatorPart(meshRenderer);
    }

    public void EnableEmissive(int number)
    {
        ElevatorManager.Instance.EnableEmissive(number);
    }

    #endregion

    #region Trophy Mechanics

    private IEnumerator TrophyMechanics()
    {
        yield return new WaitUntil(() => IsInsideSpaceLobby);

        if (TrophyManager.Instance.IsGameAccomplished((int)GameNumber.Game1))
        {
            if (TrophyManager.Instance.IsGameTrophyPresented((int)GameNumber.Game1))
            {
                Game1TrophyDisplay.SetActive(true);
                Game1TrophyLight.SetActive(true);
            }
            else
            {
                if (isTrophyGiven)
                {
                    VoiceOverManager.Instance.ButtonsInteraction(false);
                    GameNumber = GameNumber.Game1;
                    Game1Trophy.SetActive(true);
                    trophy.gameObject.SetActive(true);
                    Game1TrophyHologram.SetActive(true);
                }
                else
                {
                    if (!isGivingTrophy)
                    {
                        isGivingTrophy = true;
                        AssistantBehavior.Instance.MoveAndGiveTrophy(() => 
                        { 
                            isTrophyGiven = true;
                            AssistantBehavior.Instance.Speak(givingTrophyClip);
                        });
                    }
                }
            }
        }

        if (TrophyManager.Instance.IsGameAccomplished((int)GameNumber.Game2))
        {
            if (TrophyManager.Instance.IsGameTrophyPresented((int)GameNumber.Game2))
            {
                Game2TrophyDisplay.SetActive(true);
                Game2TrophyLight.SetActive(true);
            }
            else
            {
                if (isTrophyGiven)
                {
                    VoiceOverManager.Instance.ButtonsInteraction(false);
                    GameNumber = GameNumber.Game2;
                    Game2Trophy.SetActive(true);
                    trophy.gameObject.SetActive(true);
                    Game2TrophyHologram.SetActive(true);
                }
                else
                {
                    if (!isGivingTrophy)
                    {
                        isGivingTrophy = true;
                        AssistantBehavior.Instance.MoveAndGiveTrophy(() =>
                        {
                            isTrophyGiven = true;
                            AssistantBehavior.Instance.Speak(givingTrophyClip);
                        });
                    }
                }
            }
        }

        if (TrophyManager.Instance.IsGameAccomplished((int)GameNumber.Game3))
        {
            if (TrophyManager.Instance.IsGameTrophyPresented((int)GameNumber.Game3))
            {
                Game3TrophyDisplay.SetActive(true);
                Game3TrophyLight.SetActive(true);
            }
            else
            {
                if (isTrophyGiven)
                {
                    VoiceOverManager.Instance.ButtonsInteraction(false);
                    GameNumber = GameNumber.Game3;
                    Game3Trophy.SetActive(true);
                    trophy.gameObject.SetActive(true);
                    Game3TrophyHologram.SetActive(true);
                }
                else
                {
                    if (!isGivingTrophy)
                    {
                        isGivingTrophy = true;
                        AssistantBehavior.Instance.MoveAndGiveTrophy(() =>
                        {
                            isTrophyGiven = true;
                            AssistantBehavior.Instance.Speak(givingTrophyClip);
                        });
                    }
                }
            }
        }
    }

    #endregion
}