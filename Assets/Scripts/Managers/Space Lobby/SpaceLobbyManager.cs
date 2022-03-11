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

    [SerializeField] private TrophyGiven trophyGiven = null;
    [SerializeField] private GameObject game1TrophyGiven = null;
    [SerializeField] private GameObject game2TrophyGiven = null;
    [SerializeField] private GameObject game3TrophyGiven = null;
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
    private TrophyType trophyType = TrophyType.None;

    #endregion

    #region Encapsulations

    public GameObject Game1TrophyGiven { get => game1TrophyGiven; }
    public GameObject Game2TrophyGiven { get => game2TrophyGiven; }
    public GameObject Game3TrophyGiven { get => game3TrophyGiven; }
    public GameObject Game1TrophyDisplay { get => game1TrophyDisplay; }
    public GameObject Game2TrophyDisplay { get => game2TrophyDisplay; }
    public GameObject Game3TrophyDisplay { get => game3TrophyDisplay; }
    public GameObject Game1TrophyHologram { get => game1TrophyHologram; }
    public GameObject Game2TrophyHologram { get => game2TrophyHologram; }
    public GameObject Game3TrophyHologram { get => game3TrophyHologram; }
    public GameObject Game1TrophyLight { get => game1TrophyLight; }
    public GameObject Game2TrophyLight { get => game2TrophyLight; }
    public GameObject Game3TrophyLight { get => game3TrophyLight; }
    public TrophyType TrophyType { get => trophyType; set => trophyType = value; }

    #endregion

    #region Trophy Mechanics

    private void Start()
    {
        if (TrophyManager.Instance.IsGame1Accomplished)
        {
            if (TrophyManager.Instance.IsGame1TrophyPresented)
            {
                Game1TrophyDisplay.SetActive(true);
                Game1TrophyLight.SetActive(true);
            }
            else
            {
                TrophyType = TrophyType.Game1;
                Game1TrophyGiven.SetActive(true);
                trophyGiven.gameObject.SetActive(true);
                Game1TrophyHologram.SetActive(true);
                TrophyManager.Instance.IsGame1TrophyPresented = true;
            }
        }

        if (TrophyManager.Instance.IsGame2Accomplished)
        {
            if (TrophyManager.Instance.IsGame2TrophyPresented)
            {
                Game2TrophyDisplay.SetActive(true);
                Game2TrophyLight.SetActive(true);
            }
            else
            {
                TrophyType = TrophyType.Game2;
                Game2TrophyGiven.SetActive(true);
                trophyGiven.gameObject.SetActive(true);
                Game2TrophyHologram.SetActive(true);
                TrophyManager.Instance.IsGame2TrophyPresented = true;
            }
        }

        if (TrophyManager.Instance.IsGame3Accomplished)
        {
            if (TrophyManager.Instance.IsGame3TrophyPresented)
            {
                Game3TrophyDisplay.SetActive(true);
                Game3TrophyLight.SetActive(true);
            }
            else
            {
                TrophyType = TrophyType.Game3;
                Game3TrophyGiven.SetActive(true);
                trophyGiven.gameObject.SetActive(true);
                Game3TrophyHologram.SetActive(true);
                TrophyManager.Instance.IsGame3TrophyPresented = true;
            }
        }
    }

    #endregion

    #region Spaceship Hologram Rotation

    private void Update()
    {
        spaceshipHologram.transform.Rotate(0, 0, 10 * Time.deltaTime);
    }

    #endregion

    #region Elevator Button

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
}

public enum TrophyType
{
    None,
    Game1,
    Game2,
    Game3,
}