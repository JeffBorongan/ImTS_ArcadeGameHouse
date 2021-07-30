using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BowlingGameManagement : GameManagement
{
    #region Singleton

    public static BowlingGameManagement Instance { private set; get; }

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

    private BowlingSessionData sessionData = null;

    [Header("Enemy Spawning")]
    [SerializeField] private Transform cockpitPosition = null;
    [SerializeField] private List<SpawnPoint> enemySpawnPoints = new List<SpawnPoint>();

    Dictionary<KeyValuePair<Side, Side>, bool> lanesStatus = new Dictionary<KeyValuePair<Side, Side>, bool>();
    System.Random SpawnPointGenerator = new System.Random(DateTime.Now.Ticks.GetHashCode());
    List<int> spawnPointsGenerated = new List<int>();

    private List<GameObject> spawnedAliens = new List<GameObject>();

    private IEnumerator spawningCour = null;
    private bool isSpawning = false;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI txtCountdownTimer = null;
    [SerializeField] private TextMeshProUGUI txtTimer = null;
    [SerializeField] private TextMeshProUGUI txtGameResult = null;
    [SerializeField] private TextMeshProUGUI txtPoints = null;

    private int currentPoints = 0;
    private int currentEnemyReachedCockpit = 0;

    private IEnumerator countdownTimerCour = null;
    private IEnumerator gameTimerCour = null;

    [Header("Environment")]
    [SerializeField] private Transform leftGate = null;
    [SerializeField] private Transform rightGate = null;

    #endregion

    private void Start()
    {
        StartGame(new BowlingSessionData(), () =>
        {
            Debug.Log("Game End");
        });
    }

    #region Game Start

    public override void StartGame(SessionData data, UnityAction OnEndGame)
    {
        sessionData = (BowlingSessionData)data;

        countdownTimerCour = TimeCour(3, txtCountdownTimer, () =>
        {
            SetGate(true);
            StartSpawingEnemy();

            txtPoints.text = currentPoints.ToString();
            txtPoints.gameObject.SetActive(true);

            gameTimerCour = TimeCour(sessionData.timeDuration, txtTimer, () =>
            {
                SetGate(false);
                ShowGameResult(false);
                OnEndGame.Invoke();
            });

            StartCoroutine(gameTimerCour);

        });

        StartCoroutine(countdownTimerCour);
    }

    #endregion

    #region Enemy Spawning

    private void StartSpawingEnemy()
    {
        isSpawning = true;

        SpawnPointGenerator = new System.Random(DateTime.Now.Ticks.GetHashCode());
        spawnPointsGenerated.Clear();
        lanesStatus.Clear();

        foreach (var lane in sessionData.lanes)
        {
            for (int i = 0; i < (int)Side.Count; i++)
            {
                lanesStatus.Add(new KeyValuePair<Side, Side>(lane, (Side)i), false);
            }
        }

        spawningCour = SpawningCour();
        StartCoroutine(spawningCour);
    }

    IEnumerator SpawningCour()
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

            yield return new WaitForSeconds(sessionData.enemySpawnTime);
        }
    }

    private void SpawnEnemy(Side lane, Side side, Transform point, int index)
    {
        GameObject clone = ObjectPooling.Instance.GetFromPool(TypeOfObject.EnemyAlien);
        clone.transform.position = point.position;
        clone.transform.rotation = point.rotation;
        clone.SetActive(true);
        spawnedAliens.Add(clone);

        AlienMovement alien = clone.GetComponent<AlienMovement>();
        alien.SetMovementSpeed(sessionData.enemySpeed);
        alien.PathPoint = cockpitPosition;
        alien.GoToTheCockpit();

        alien.OnDeath.AddListener(() =>
        {
            lanesStatus[new KeyValuePair<Side, Side>(lane, side)] = false;
            spawnPointsGenerated.Remove(index);
            AddPoint();

            alien.OnDeath.RemoveAllListeners();
        });

        alien.OnReachDestination.AddListener(() =>
        {
            lanesStatus[new KeyValuePair<Side, Side>(lane, side)] = false;
            spawnPointsGenerated.Remove(index);
            AddEnemyReachedCockpit();

            alien.OnReachDestination.RemoveAllListeners();
        });

        lanesStatus[new KeyValuePair<Side, Side>(lane, side)] = true;
    }


    public void SpawnOneEnemy(Side main, Side secondary, SpawnEnemyAction action, UnityAction OnDeathSpawn, UnityAction OnReachCockpit)
    {
        SpawnPoint newSpawnPoint = enemySpawnPoints.Where(s => s.lane == main && s.side == secondary).FirstOrDefault();
        GameObject clone = ObjectPooling.Instance.GetFromPool(TypeOfObject.EnemyAlien);
        clone.transform.position = newSpawnPoint.point.position;
        clone.transform.rotation = newSpawnPoint.point.rotation;
        clone.SetActive(true);
        spawnedAliens.Add(clone);

        AlienMovement alien = clone.GetComponent<AlienMovement>();
        alien.SetMovementSpeed(action.enemySpeed);
        alien.PathPoint = cockpitPosition;
        alien.GoToTheCockpit();

        alien.OnDeath.AddListener(() =>
        {
            OnDeathSpawn.Invoke();
            alien.OnDeath.RemoveAllListeners();
        });

        alien.OnReachDestination.AddListener(() =>
        {
            OnReachCockpit.Invoke();
            alien.OnReachDestination.RemoveAllListeners();
        });
    }

    #endregion

    #region Timer

    IEnumerator TimeCour(int timerDuration, TextMeshProUGUI txt, UnityAction OnEndTimer)
    {
        txt.gameObject.SetActive(true);
        int currentTime = timerDuration;
        while (currentTime >= 0)
        {
            txt.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        txt.gameObject.SetActive(false);
        OnEndTimer.Invoke();
    }

    #endregion

    #region Points

    private void AddPoint()
    {
        if(currentPoints + 1 < sessionData.pointsToEarn)
        {
            currentPoints++;
            txtPoints.text = currentPoints.ToString();
            if(currentPoints == sessionData.pointsToEarn)
            {
                ShowGameResult(true);
            }
        }
    }

    private void AddEnemyReachedCockpit()
    {
        if(currentEnemyReachedCockpit + 1 < sessionData.enemyReachedTheCockpit)
        {
            currentEnemyReachedCockpit++;
            if(currentEnemyReachedCockpit == sessionData.enemyReachedTheCockpit)
            {
                ShowGameResult(false);
            }
        }
    }

    private void ShowGameResult(bool success)
    {
        txtGameResult.text = success ? "Success" : "Failed";
        txtGameResult.gameObject.SetActive(true);
    }

    #endregion

    #region Environment

    private void SetGate(bool open)
    {
        if (open)
        {
            leftGate.DOLocalMoveX(7.34f, 1f);
            rightGate.DOLocalMoveX(-0.329f, 1f);
        }
        else
        {
            leftGate.DOLocalMoveX(3.568514f, 1f);
            rightGate.DOLocalMoveX(3.568514f, 1f);
        }
    }

    #endregion
}

public class BowlingSessionData : SessionData
{
    public float enemySpawnTime = 5f;
    public List<Side> lanes = new List<Side>() { Side.Left, Side.Middle, Side.Right };
    public float enemySpeed = 0.5f;
    public int pointPerEnemy = 1;

    public int pointsToEarn = 10;
    public int timeDuration = 240;

    public int enemyReachedTheCockpit = 1;
}
