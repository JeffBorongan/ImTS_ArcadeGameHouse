using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class WhackGameManagement : GameManagement
{
    #region Singleton

    public static WhackGameManagement Instance { private set; get; }

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

    private WhackGameSessionData sessionData = null;

    [Header("Enemy Spawning")]
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private Transform enemyDestinationPoint;
    private IEnumerator spawningCour = null;
    private bool isSpawning = false;

    [Header("Game Mechanics")]
    [SerializeField] private GameObject ovalRoom;

    [Header("UI")]
    [SerializeField] private GameObject pnlHUD = null;
    [SerializeField] private GameObject pnlGameResult = null;
    [SerializeField] private TextMeshProUGUI txtCountdownTimer = null;
    [SerializeField] private TextMeshProUGUI txtEndResult = null;
    [SerializeField] private Color colorSuccessText = Color.blue;
    [SerializeField] private Color colorFailedText = Color.blue;
    private IEnumerator countdownTimerCour = null;

    private AlienMovement alien = null;
    private IEnumerator enemyCheckingCour = null;
    private int currentEnemyReachedThePlayer = 0;

    #endregion

    #region Initialize

    public override void InitializeGame()
    {
        StartGame(new WhackGameSessionData(), () => { });
    }

    #endregion

    #region Game Start

    public override void StartGame(SessionData data, UnityAction OnEndGame)
    {
        pnlStartGame.gameObject.SetActive(true);
        btnStartGame.onClick.RemoveAllListeners();
        btnStartGame.onClick.AddListener(() =>
        {
            sessionData = (WhackGameSessionData)data;
            countdownTimerCour = TimeCour(3, txtCountdownTimer, () =>
            {
                ElevatorFloorManager.Instance.characterPrefab.GetComponent<NavMeshObstacle>().enabled = false;
                isSpawning = true;
                spawningCour = SpawningCour(sessionData.enemySpawnInterval);
                StartCoroutine(spawningCour);

                enemyCheckingCour = EnemyCheckingCour(() =>
                {
                    StopGame();
                    OnEndGame.Invoke();
                    OnGameEnd.Invoke();
                });

                StartCoroutine(enemyCheckingCour);
            });

            StartCoroutine(countdownTimerCour);
            pnlStartGame.gameObject.SetActive(false);
        });
    }

    public override void StopGame()
    {
        ElevatorFloorManager.Instance.characterPrefab.GetComponent<NavMeshObstacle>().enabled = true;
        isSpawning = false;
        StopCoroutine(enemyCheckingCour);
        StopCoroutine(spawningCour);
    }

    #endregion

    #region Game Update

    private void Update()
    {
        if (isSpawning)
        {
            ovalRoom.transform.Rotate(Vector3.up * sessionData.roomMoveSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region Enemy Spawning

    IEnumerator SpawningCour(float waitTime)
    {
        while (isSpawning)
        {
            SpawnEnemy(enemySpawnPoint, enemyDestinationPoint);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnEnemy(Transform spawnPoint, Transform enemyGoal)
    {
        GameObject clone = ObjectPooling.Instance.GetFromPool(TypeOfObject.EnemyAlien);
        clone.transform.position = spawnPoint.position;
        clone.transform.rotation = spawnPoint.rotation;
        clone.SetActive(true);

        alien = clone.GetComponent<AlienMovement>();
        alien.SetMovementSpeed(sessionData.enemyMoveSpeed);
        alien.AlienAgent.SetDestination(enemyGoal.position);

        alien.OnDeath.AddListener(() =>
        {
            alien.OnDeath.RemoveAllListeners();
        });

        alien.OnReachDestination.AddListener(() =>
        {
            AddEnemyReachedThePlayer();
            alien.OnReachDestination.RemoveAllListeners();
        });
    }

    #endregion

    #region Enemy Status Checking

    IEnumerator EnemyCheckingCour(UnityAction OnEndGame)
    {
        yield return new WaitUntil(() => currentEnemyReachedThePlayer >= sessionData.enemyReachedThePlayer);
        OnEndGame.Invoke();
    }

    private void AddEnemyReachedThePlayer()
    {
        currentEnemyReachedThePlayer++;
        if (currentEnemyReachedThePlayer == sessionData.enemyReachedThePlayer)
        {
            StopGame();
            ShowGameResult(false);
            OnGameEnd.Invoke();
        }
    }

    #endregion

    #region UI

    IEnumerator TimeCour(int timerDuration, TextMeshProUGUI txt, UnityAction OnEndTimer, bool inMinutes = false)
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
    }

    #endregion
}

public class WhackGameSessionData : SessionData
{
    public float roomMoveSpeed = 3f;

    public int enemyReachedThePlayer = 1;
    public float enemySpawnInterval = 5f;
    public float enemyMoveSpeed = 3f;
}