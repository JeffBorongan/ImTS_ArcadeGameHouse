using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WhackGameManager : GameManagement
{
    #region Singleton

    public static WhackGameManager Instance { private set; get; }

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

    [Header("Enemy")]
    private IEnumerator enemyCheckingCour = null;
    private IEnumerator spawningCour = null;
    private int currentEnemyReachedThePlayer = 0;
    private bool isSpawning = false;

    [Header("Player")]
    [SerializeField] private Transform playerAttachment = null;
    [SerializeField] private Transform playerLocation = null;

    [Header("UI")]
    [SerializeField] private GameObject uIPnl = null;
    [SerializeField] private GameObject pnlHUD = null;
    [SerializeField] private GameObject pnlGameResult = null;
    [SerializeField] private TextMeshProUGUI txtCountdownTimer = null;
    [SerializeField] private TextMeshProUGUI txtEndResult = null;
    [SerializeField] private Color colorSuccessText = Color.blue;
    [SerializeField] private Color colorFailedText = Color.blue;
    private IEnumerator countdownTimerCour = null;

    [SerializeField] private GameObject vFXConfetti = null;
    [SerializeField] private AudioClip gameSuccessClip = null;
    [SerializeField] private AudioClip gameFailClip = null;
    private WhackGameSessionData sessionData = null;

    System.Random SpawnPointGenerator = new System.Random(DateTime.Now.Ticks.GetHashCode());
    List<int> spawnPointsGenerated = new List<int>();
    Dictionary<KeyValuePair<Side, Side>, bool> lanesStatus = new Dictionary<KeyValuePair<Side, Side>, bool>();
    [SerializeField] private List<SpawnPoint> enemySpawnPoints = new List<SpawnPoint>();
    private List<GameObject> spawnedAliens = new List<GameObject>();
    private IEnumerator pointCheckingCour = null;
    private int currentPoints = 0;

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    private int spawnIndex = 0;

    #endregion

    #region Encapsulations

    public WhackGameSessionData SessionData { get => sessionData; set => sessionData = value; }

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
            UXManager.Instance.HandleOnWhackGameStart();
            CharacterManager.Instance.PointersVisibility(false);
            countdownTimerCour = TimeCour(3, txtCountdownTimer, () =>
            {
                CharacterManager.Instance.CharacterPrefab.transform.SetParent(playerLocation);
                uIPnl.transform.SetParent(playerLocation);
                StartSpawningEnemy();
                //SpawnPrepare();
                //isSpawning = true;

                pointCheckingCour = PointCheckingCour(() =>
                {
                    StopGame();
                    OnEndGame.Invoke();
                    OnGameEnd.Invoke();
                });

                enemyCheckingCour = EnemyCheckingCour(() =>
                {
                    StopGame();
                    OnEndGame.Invoke();
                    OnGameEnd.Invoke();
                });

                StartCoroutine(pointCheckingCour);
                StartCoroutine(enemyCheckingCour);
            });

            StartCoroutine(countdownTimerCour);
            pnlStartGame.gameObject.SetActive(false);
        });
    }

    public override void StopGame()
    {
        foreach (var alien in spawnedAliens)
        {
            alien.SetActive(false);
        }

        isSpawning = false;
        StopCoroutine(pointCheckingCour);
        StopCoroutine(enemyCheckingCour);
        StopCoroutine(spawningCour);
    }

    #endregion

    #region Update

    private void Update()
    {
        if (isSpawning)
        {
            playerAttachment.Rotate(-(SessionData.playerSpeed / SessionData.playerSpeedFactor) * Time.deltaTime * Vector3.up);
        }
    }

    #endregion

    #region Enemy Spawning

    private void SpawnPrepare()
    {
        for (; spawnIndex < spawnPoints.Count / 2; spawnIndex++)
        {
            SpawnEnemy(spawnPoints[spawnIndex]);
        }
    }

    private void StartSpawningEnemy()
    {
        isSpawning = true;
        SpawnPointGenerator = new System.Random(DateTime.Now.Ticks.GetHashCode());
        spawnPointsGenerated.Clear();
        lanesStatus.Clear();

        foreach (var lane in SessionData.lanes)
        {
            for (int i = 0; i < (int)Side.Count; i++)
            {
                lanesStatus.Add(new KeyValuePair<Side, Side>(lane, (Side)i), false);
            }
        }

        spawningCour = SpawningCour();
        StartCoroutine(spawningCour);
    }

    private IEnumerator SpawningCour()
    {
        while (isSpawning)
        {
            if (spawnPointsGenerated.Count != lanesStatus.Count)
            {
                int randomSpawnPointIndex = SpawnPointGenerator.Next(lanesStatus.Count);

                while (spawnPointsGenerated.Contains(randomSpawnPointIndex))
                {
                    randomSpawnPointIndex = SpawnPointGenerator.Next(lanesStatus.Count);
                }

                spawnPointsGenerated.Add(randomSpawnPointIndex);
                var random = lanesStatus.ElementAt(randomSpawnPointIndex);

                if (!random.Value)
                {
                    SpawnPoint newSpawnPoint = enemySpawnPoints.Where(s => s.lane == random.Key.Key && s.side == random.Key.Value).FirstOrDefault();

                    SpawnEnemy(newSpawnPoint.lane, newSpawnPoint.side, newSpawnPoint.point, randomSpawnPointIndex);
                }
            }

            yield return new WaitForSeconds(SessionData.enemySpawnInterval);
        }
    }

    private void SpawnEnemy(Side lane, Side side, Transform point, int index)
    {
        GameObject clone = ObjectPoolingManager.Instance.GetFromPool(TypeOfObject.Alien3);
        clone.transform.SetPositionAndRotation(point.position, point.rotation);
        clone.SetActive(true);
        spawnedAliens.Add(clone);

        AlienMovement alien = clone.GetComponent<AlienMovement>();
        alien.AlienAgent.SetDestination(playerLocation.transform.position);

        alien.OnDeath.AddListener(() =>
        {
            StartCoroutine(FunctionWithDelay(5f, () =>
            {
                lanesStatus[new KeyValuePair<Side, Side>(lane, side)] = false;
                spawnPointsGenerated.Remove(index);
            }));

            AddPoint();
            alien.OnDeath.RemoveAllListeners();
        });

        alien.OnReachDestination.AddListener(() =>
        {
            StartCoroutine(FunctionWithDelay(5f, () =>
            {
                lanesStatus[new KeyValuePair<Side, Side>(lane, side)] = false;
                spawnPointsGenerated.Remove(index);
            }));

            AddEnemyReachedThePlayer();
            alien.gameObject.SetActive(false);

            alien.OnReachDestination.RemoveAllListeners();
        });

        lanesStatus[new KeyValuePair<Side, Side>(lane, side)] = true;
    }

    private void SpawnEnemy(Transform point)
    {
        GameObject clone = ObjectPoolingManager.Instance.GetFromPool(spawnIndex % 2 == 0 ? TypeOfObject.Alien4 : TypeOfObject.Alien3);
        clone.transform.SetPositionAndRotation(point.position, point.rotation);
        clone.SetActive(true);
        spawnedAliens.Add(clone);

        AlienMovement alien = clone.GetComponent<AlienMovement>();
        alien.AlienAgent.SetDestination(playerLocation.transform.position);

        alien.OnDeath.AddListener(() =>
        {
            Debug.Log(alien.name);
            AddPoint();
            //SpawnCheck();
            alien.OnDeath.RemoveAllListeners();
        });

        alien.OnReachDestination.AddListener(() =>
        {
            AddEnemyReachedThePlayer();
            //SpawnCheck();
            alien.gameObject.SetActive(false);
            alien.OnReachDestination.RemoveAllListeners();
        });
    }

    private void SpawnCheck()
    {
        if (spawnIndex + 1 < spawnPoints.Count)
        {
            spawnIndex++;
        }
        else
        {
            spawnIndex = 0;
        }

        SpawnEnemy(spawnPoints[spawnIndex]);
    }

    #endregion

    #region Enemy Status Checking

    IEnumerator PointCheckingCour(UnityAction OnEndGame)
    {
        yield return new WaitUntil(() => currentPoints >= SessionData.aliensToHit);
        OnEndGame.Invoke();
    }

    private void AddPoint()
    {
        if (currentPoints <= SessionData.aliensToHit)
        {
            currentPoints++;

            if (currentPoints == SessionData.aliensToHit)
            {
                ShowGameResult(true);
            }
        }
    }

    private IEnumerator EnemyCheckingCour(UnityAction OnEndGame)
    {
        yield return new WaitUntil(() => currentEnemyReachedThePlayer >= SessionData.enemyReachedThePlayer);
        OnEndGame.Invoke();
    }

    public void AddEnemyReachedThePlayer()
    {
        currentEnemyReachedThePlayer++;
        if (currentEnemyReachedThePlayer == SessionData.enemyReachedThePlayer)
        {
            ShowGameResult(false);
        }
    }

    #endregion

    #region UI

    private IEnumerator TimeCour(int timerDuration, TextMeshProUGUI txt, UnityAction OnEndTimer, bool inMinutes = false)
    {
        txt.gameObject.SetActive(true);
        int currentTime = timerDuration;
        while (currentTime >= 0)
        {
            int minutes = currentTime / 60;
            int seconds = currentTime - (minutes * 60);
            txt.text = inMinutes ? minutes + ":" + seconds : currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        txt.gameObject.SetActive(false);
        OnEndTimer.Invoke();
    }

    private void ShowGameResult(bool success)
    {
        pnlHUD.SetActive(false);
        pnlGameResult.SetActive(true);
        txtEndResult.text = success ? "Success" : "Failed";
        txtEndResult.color = success ? colorSuccessText : colorFailedText;
        vFXConfetti.SetActive(success);
        VoiceOverManager.Instance.ButtonsInteraction(false);

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

                CharacterManager.Instance.PointersVisibility(true);
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
    public float playerSpeedFactor = 3.6f;

    public int enemyReachedThePlayer = 5;

    public List<Side> lanes = new List<Side>() { Side.Left, Side.Middle, Side.Right };
    public int aliensToHit = 10;
    public float enemySpawnInterval = 1f;
}