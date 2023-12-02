using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TileGameManager : GameManagement
{
    #region Singleton

    public static TileGameManager Instance { private set; get; }

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

    [Header("Mechanics")]
    [SerializeField] private Transform tileSpawnPoint = null;
    [SerializeField] private MeshRenderer floorMeshRenderer = null;
    [SerializeField] private List<Texture2D> floorBaseMaps = new List<Texture2D>();
    [SerializeField] private List<Texture2D> floorNormalMaps = new List<Texture2D>();
    [SerializeField] private List<Texture2D> floorEmissionMaps = new List<Texture2D>();
    [SerializeField] private List<Texture2D> tileBaseMaps = new List<Texture2D>();
    [SerializeField] private List<Texture2D> tileNormalMaps = new List<Texture2D>();
    [SerializeField] private List<Texture2D> tileEmissionMaps = new List<Texture2D>();
    private List<GameObject> spawnedTiles = new List<GameObject>();
    private List<GameObject> spawnedSpaceships = new List<GameObject>();
    private List<TileColor> tileColorList = new List<TileColor>();
    private TileColor tileColor = TileColor.None;
    private IEnumerator pointCheckingCour = null;
    private bool isMoving = false;
    private float travelDistance = 0f;
    private int degreeMovement = 359;
    private int tilesPassed = 0;
    private int spaceshipCount = 0;

    [Header("Player")]
    [SerializeField] private Transform playerAttachment = null;
    [SerializeField] private Transform playerLocation = null;

    [Header("UI")]
    [SerializeField] private GameObject uIPnl = null;
    [SerializeField] private GameObject pnlHUD = null;
    [SerializeField] private GameObject pnlGameResult = null;
    [SerializeField] private Image imgTimerIcon = null;
    [SerializeField] private TextMeshProUGUI txtTime = null;
    [SerializeField] private TextMeshProUGUI txtTilesPassed = null;
    [SerializeField] private TextMeshProUGUI txtEndTime = null;
    [SerializeField] private TextMeshProUGUI txtEndTilesPassed = null;
    [SerializeField] private TextMeshProUGUI txtCountdownTimer = null;
    [SerializeField] private TextMeshProUGUI txtEndResult = null;
    [SerializeField] private Color colorSuccessText = Color.blue;
    [SerializeField] private Color colorFailedText = Color.blue;
    private IEnumerator countdownTimerCour = null;
    private IEnumerator gameTimerCour = null;

    [Space, Space, Space]
    [SerializeField] private GameObject vFXConfetti = null;
    [SerializeField] private GameObject vFXGreen = null;
    [SerializeField] private GameObject vFXOrange = null;
    [SerializeField] private GameObject vFXPink = null;
    [SerializeField] private AudioClip gameSuccessClip = null;
    [SerializeField] private AudioClip gameFailClip = null;
    private WhackGameSessionData sessionData = null;

    #endregion

    #region Encapsulations

    public WhackGameSessionData SessionData { get => sessionData; set => sessionData = value; }
    public MeshRenderer FloorMeshRenderer { get => floorMeshRenderer; }
    public List<Texture2D> FloorBaseMaps { get => floorBaseMaps; }
    public List<Texture2D> FloorNormalMaps { get => floorNormalMaps; }
    public List<Texture2D> FloorEmissionMaps { get => floorEmissionMaps; }
    public TextMeshProUGUI TxtTime { get => txtTime; }
    public TextMeshProUGUI TxtTilesPassed { get => txtTilesPassed; }
    public int SpaceshipCount { get => spaceshipCount; set => spaceshipCount = value; }
    public GameObject VFXGreen { get => vFXGreen; }
    public GameObject VFXOrange { get => vFXOrange; }
    public GameObject VFXPink { get => vFXPink; }

    #endregion

    #region Initialize

    public override void InitializeGame()
    {
        StartGame(new WhackGameSessionData(), () => { });
    }

    #endregion

    #region Start

    public override void StartGame(SessionData data, UnityAction OnEndGame)
    {
        SessionData = (WhackGameSessionData)data;
        btnStartGame.onClick.RemoveAllListeners();
        btnStartGame.onClick.AddListener(() =>
        {
            UXManager.Instance.HandleOnTileGameStart();
            CharacterManager.Instance.PointersVisibility(false);
            countdownTimerCour = TimeCour(false, 3, txtCountdownTimer, () =>
            {
                CharacterManager.Instance.CharacterPrefab.transform.SetParent(playerLocation);
                uIPnl.transform.SetParent(playerLocation);
                txtTilesPassed.text = tilesPassed.ToString();
                imgTimerIcon.gameObject.SetActive(true);
                txtTilesPassed.gameObject.SetActive(true);
                tileColorList.Add(TileColor.Green);
                tileColorList.Add(TileColor.Orange);
                tileColorList.Add(TileColor.Pink);
                SpawnTiles();
                isMoving = true;
                gameTimerCour = TimeCour(true, 0, txtTime, () => { });

                pointCheckingCour = PointCheckingCour(() =>
                {
                    StopGame();
                    OnEndGame.Invoke();
                    OnGameEnd.Invoke();
                });

                StartCoroutine(gameTimerCour);
                StartCoroutine(pointCheckingCour);
                UXManager.Instance.BtnTileGameStop.interactable = true;
                UXManager.Instance.IsTileGameStarted = true;
            });

            StartCoroutine(countdownTimerCour);
            pnlStartGame.gameObject.SetActive(false);
        });
    }

    public override void StopGame()
    {
        foreach (var item in spawnedTiles)
        {
            item.SetActive(false);
        }

        foreach (var item in spawnedSpaceships)
        {
            item.SetActive(false);
        }

        isMoving = false;
        StopCoroutine(gameTimerCour);
        StopCoroutine(pointCheckingCour);
    }

    #endregion

    #region Update

    private void Update()
    {
        if (isMoving)
        {
            playerAttachment.Rotate(-SessionData.playerSpeed * SessionData.speedFactor * Time.deltaTime * Vector3.up);

            if (playerAttachment.transform.eulerAngles.y <= degreeMovement)
            {
                travelDistance += (SessionData.roomCircumference / 360);
                travelDistance = Mathf.Round(travelDistance * 100f) / 100f;
                UXManager.Instance.TxtTravelDistance.text = travelDistance.ToString() + " m";
                degreeMovement--;

                if (degreeMovement <= 0)
                {
                    degreeMovement = 359;
                }
            }
        }
    }

    #endregion

    #region Timer

    private IEnumerator TimeCour(bool isProgressiveTimer, int timerDuration, TextMeshProUGUI txt, UnityAction OnEndTimer)
    {
        txt.gameObject.SetActive(true);
        int currentTime = timerDuration;

        if (isProgressiveTimer)
        {
            while (isMoving)
            {
                string separator = ":";
                int minutes = currentTime / 60;
                int seconds = currentTime - (minutes * 60);

                if (seconds < 10)
                {
                    separator = ":0";
                }

                txt.text = minutes + separator + seconds;
                yield return new WaitForSeconds(1f);
                currentTime++;
            }
        } 
        else
        {
            while (currentTime >= 0)
            {
                txt.text = currentTime.ToString();
                yield return new WaitForSeconds(1f);
                currentTime--;
            }
        }

        txt.gameObject.SetActive(false);
        OnEndTimer.Invoke();
    }

    #endregion

    #region Tile and Spaceship Spawning

    private void SpawnTiles()
    {
        if (spawnedTiles != null)
        {
            foreach (var item in spawnedTiles)
            {
                item.SetActive(false);
            }

            spawnedTiles.Clear();
        }

        float degreesPerTile = (360 / SessionData.roomCircumference) * SessionData.distancePerTile;
        int numberOfTiles = Mathf.RoundToInt(SessionData.roomCircumference / SessionData.distancePerTile);

        for (int index = 1; index <= numberOfTiles; index++)
        {
            bool tileRepeat = true;
            int colorNumber;
            int spaceshipVisibility = 0;

            while (tileRepeat)
            {
                colorNumber = Random.Range(0, (int)TileColor.Count);

                if ((int)tileColor != colorNumber)
                {
                    foreach (var color in tileColorList)
                    {
                        if ((int)color == colorNumber)
                        {
                            tileRepeat = false;
                            tileColor = color;
                            spaceshipVisibility = Random.Range(1, 10);
                        }
                    }
                }
            }

            GameObject tile = ObjectPoolingManager.Instance.GetFromPool(TypeOfObject.Tiles);
            tile.GetComponent<Tile>().TileMeshRenderer.material.SetTexture("_BaseMap", tileBaseMaps[(int)tileColor]);
            tile.GetComponent<Tile>().TileMeshRenderer.material.SetTexture("_BumpMap", tileNormalMaps[(int)tileColor]);
            tile.GetComponent<Tile>().TileMeshRenderer.material.SetTexture("_EmissionMap", tileEmissionMaps[(int)tileColor]);
            tile.GetComponent<Tile>().TileColor = tileColor;
            tile.transform.SetParent(tileSpawnPoint);
            tile.transform.eulerAngles = new Vector3(0, index * -degreesPerTile, 0);
            tile.SetActive(true);
            spawnedTiles.Add(tile);

            if (spaceshipVisibility >= 6)
            {
                int placement = Random.Range(-1, 1);
                GameObject spaceship = ObjectPoolingManager.Instance.GetFromPool(TypeOfObject.Spaceship);
                spaceship.transform.SetParent(tileSpawnPoint);
                spaceship.transform.position += new Vector3(0, 0, placement * 1.5f);
                spaceship.transform.eulerAngles = new Vector3(0, index * -degreesPerTile, 0);
                spaceship.SetActive(true);
                spawnedSpaceships.Add(spaceship);
            }
        }
    }

    #endregion

    #region Point Status Checking

    private IEnumerator PointCheckingCour(UnityAction OnEndGame)
    {
        yield return new WaitUntil(() => tilesPassed >= SessionData.numberOfTiles);
        OnEndGame.Invoke();
    }

    public void AddTilePassed()
    {
        if (tilesPassed <= SessionData.numberOfTiles)
        {
            tilesPassed++;
            txtTilesPassed.text = tilesPassed + "/" + SessionData.numberOfTiles;

            if (tilesPassed == SessionData.numberOfTiles)
            {
                GameResult(true);
            }
        }
    }

    #endregion

    #region Game Result

    private void GameResult(bool success)
    {
        pnlHUD.SetActive(false);
        pnlGameResult.SetActive(true);

        txtEndTilesPassed.text = txtTilesPassed.text;
        txtEndTime.text = txtTime.text;
        txtEndResult.text = success ? "Success" : "Failed";
        txtEndTilesPassed.color = success ? txtEndTilesPassed.color : colorFailedText;
        txtEndResult.color = success ? colorSuccessText : colorFailedText;

        vFXConfetti.SetActive(success);
        VFXGreen.SetActive(false);
        VFXOrange.SetActive(false);
        VFXPink.SetActive(false);

        FloorMeshRenderer.material.SetTexture("_BaseMap", FloorBaseMaps[0]);
        FloorMeshRenderer.material.SetTexture("_BumpMap", FloorNormalMaps[0]);
        FloorMeshRenderer.material.SetTexture("_EmissionMap", FloorEmissionMaps[0]);

        UXManager.Instance.BtnTileGameStop.interactable = false;
        UXManager.Instance.IsTileGameStarted = false;

        VoiceOverManager.Instance.ButtonsInteraction(false);
        VoiceOverManager.Instance.ToggleGame3ExtraButtons(false);

        if (success)
        {
            VoiceOverManager.Instance.PlayClip(gameSuccessClip);
            StartCoroutine(FunctionWithDelay(gameSuccessClip.length, () =>
            {
                vFXConfetti.SetActive(false);

                CharacterManager.Instance.CharacterPrefab.transform.SetParent(CharacterManager.Instance.PlayerLocation);

                TrophyManager.Instance.AddGameAccomplished((int)GameNumber.Game3);
                TrophyManager.Instance.IsGame3Failed = false;

                VoiceOverManager.Instance.LastButtonSelected = LastButtonSelected.WalkeyMoleyToInventoryRoom;
                VoiceOverManager.Instance.InvokeLastButtonSelected();

                ElevatorManager.Instance.CloseDoorDetection = true;
                ElevatorManager.Instance.PlayerDetection = false;
            }));
        }
        else
        {
            VoiceOverManager.Instance.PlayClip(gameFailClip);
            StartCoroutine(FunctionWithDelay(gameFailClip.length, () =>
            {
                CharacterManager.Instance.CharacterPrefab.transform.SetParent(CharacterManager.Instance.PlayerLocation);
                TrophyManager.Instance.IsGame3Failed = true;
                VoiceOverManager.Instance.LastButtonSelected = LastButtonSelected.WalkeyMoleyToInventoryRoom;
                VoiceOverManager.Instance.InvokeLastButtonSelected();
                ElevatorManager.Instance.CloseDoorDetection = true;
                ElevatorManager.Instance.PlayerDetection = false;
            }));
        }
    }

    #endregion

    #region Game Stop

    public void GameStop()
    {
        StopGame();
        GameResult(false);
        OnGameEnd.Invoke();
    }

    #endregion

    #region Enable Start Button

    public void EnableStartButton()
    {
        pnlStartGame.gameObject.SetActive(true);
        CharacterManager.Instance.PointersVisibility(true);
    }

    #endregion

    #region Teleport

    public void InitiateTeleport()
    {
        StartCoroutine(TeleportCour(10f, (int)Floors.InventoryRoom));
    }

    private IEnumerator TeleportCour(float waitTime, int floor)
    {
        yield return new WaitForSeconds(waitTime);
        ScreenFadeManager.Instance.FadeIn(() =>
        {
            SceneManager.Instance.LoadFloor((Floors)floor, () =>
            {
                foreach (var item in ElevatorManager.Instance.DisableObjects)
                {
                    item.SetActive(true);
                }

                CharacterManager.Instance.CharacterPrefab.transform.SetPositionAndRotation(CharacterManager.Instance.PlayerLocation.position, Quaternion.identity);
                ScreenFadeManager.Instance.FadeOut(() => { });
            });
        });
    }

    #endregion

    #region Function with Delay

    private IEnumerator FunctionWithDelay(float waitTime, UnityAction function)
    {
        yield return new WaitForSeconds(waitTime);
        function.Invoke();
    }

    #endregion
}

public class WhackGameSessionData : SessionData
{
    public float playerSpeed = 0.2f;
    public float spaceshipSpeed = 2.5f;
    public float speedFactor = 1.27324f;
    public float roomCircumference = 77.28f;
    public int numberOfTiles = 10;
    public int distancePerTile = 5;
}