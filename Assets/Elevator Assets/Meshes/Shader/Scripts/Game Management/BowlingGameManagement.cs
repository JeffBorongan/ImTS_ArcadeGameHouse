using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    public BowlingSessionData sessionData = null;

    [Header("Enemy Spawning")]
    [SerializeField] private Transform cockpitPosition = null;
    [SerializeField] private List<SpawnPoint> enemySpawnPoints = new List<SpawnPoint>();

    Dictionary<KeyValuePair<Side, Side>, bool> lanesStatus = new Dictionary<KeyValuePair<Side, Side>, bool>();
    System.Random SpawnPointGenerator = new System.Random(DateTime.Now.Ticks.GetHashCode());
    List<int> spawnPointsGenerated = new List<int>();

    private List<GameObject> spawnedAliens = new List<GameObject>();

    private IEnumerator spawningCour = null;
    private bool isSpawning = false;
    [SerializeField] private AudioClip gameSuccessClip = null;
    [SerializeField] private AudioClip gameFailClip = null;

    [Header("UI")]
    [Header("HUD")]
    [SerializeField] private GameObject pnlHUD = null;
    [SerializeField] private TextMeshProUGUI txtCountdownTimer = null;
    [SerializeField] private Image imgTimerIcon = null;
    [SerializeField] private TextMeshProUGUI txtTimer = null;
    [SerializeField] private TextMeshProUGUI txtPoints = null;

    [Space]
    [Header("Game Result")]
    [SerializeField] private GameObject pnlGameResult = null;
    [SerializeField] private TextMeshProUGUI txtEndTime = null;
    [SerializeField] private TextMeshProUGUI txtEndPoints = null;
    [SerializeField] private TextMeshProUGUI txtEndResult = null;
    [SerializeField] private Color colorSuccessText = Color.blue;
    [SerializeField] private Color colorFailedText = Color.blue;

    private int currentPoints = 0;
    private int currentEnemyReachedCockpit = 0;

    private IEnumerator countdownTimerCour = null;
    private IEnumerator gameTimerCour = null;
    private IEnumerator pointCheckingCour = null;
    private IEnumerator enemyCheckingCour = null;

    [Header("Gate")]
    [SerializeField] private Transform leftGate = null;
    [SerializeField] private Transform rightGate = null;

    [Header("Dispensers")]
    [SerializeField] private float dispenserOffset = 0.5f;
    [SerializeField] private Transform leftBallDispenser = null;
    [SerializeField] private Transform rightBallDispenser = null;

    [Header("Player")]
    [SerializeField] private bool isPlayerLocked = false;

    [HideInInspector] public bool isBowlingGameInstructionDone = false;

    #endregion

    #region Initialize
    public override void InitializeGame()
    {
        base.InitializeGame();
        
        //GameLobbyManager.Instance.AddGame(GameID.Bowling, this);
        SpawnDispensers(CharacterManager.Instance.CurrentAnatomy);

        StartGame(new BowlingSessionData(), () => { });
    }

    private void SpawnDispensers(Dictionary<string, Vector3> newAnatomy)
    {
        Vector3 leftHandPos = newAnatomy[AnatomyPart.LeftHand.ToString()];
        Vector3 rightHandPos = newAnatomy[AnatomyPart.RightHand.ToString()];

        leftBallDispenser.transform.localPosition = new Vector3(rightHandPos.x + dispenserOffset, leftBallDispenser.transform.localPosition.y, leftBallDispenser.transform.localPosition.z);
        rightBallDispenser.transform.localPosition = new Vector3(leftHandPos.x + -dispenserOffset, rightBallDispenser.transform.localPosition.y, rightBallDispenser.transform.localPosition.z);

        leftBallDispenser.gameObject.SetActive(true);
        rightBallDispenser.gameObject.SetActive(true);
    }

    public void UpdateDispensers(Dictionary<string, Vector3> newAnatomy)
    {
        Vector3 leftHandPos = newAnatomy[AnatomyPart.LeftHand.ToString()];
        Vector3 rightHandPos = newAnatomy[AnatomyPart.RightHand.ToString()];

        leftBallDispenser.transform.localPosition = new Vector3(rightHandPos.x + sessionData.dispenserOffset, leftBallDispenser.transform.localPosition.y, leftBallDispenser.transform.localPosition.z);
        rightBallDispenser.transform.localPosition = new Vector3(leftHandPos.x + -sessionData.dispenserOffset, rightBallDispenser.transform.localPosition.y, rightBallDispenser.transform.localPosition.z);
    }

    #endregion

    #region Game Start

    public override void StartGame(SessionData data, UnityAction OnEndGame)
    {
        sessionData = (BowlingSessionData)data;
        btnStartGame.onClick.RemoveAllListeners();
        btnStartGame.onClick.AddListener(() =>
        {
            BowlingGameUXManager.Instance.HandleOnStart();
            countdownTimerCour = TimeCour(3, txtCountdownTimer, () =>
            {
                imgTimerIcon.gameObject.SetActive(true);
                SetGate(true);
                StartSpawingEnemy();

                txtPoints.text = currentPoints.ToString();
                txtPoints.gameObject.SetActive(true);

                gameTimerCour = TimeCour(sessionData.timeDuration, txtTimer, () =>
                {
                    StopGame();
                    SetGate(false);
                    ShowGameResult(false);
                    OnEndGame.Invoke();
                    OnGameEnd.Invoke();
                }, true);

                pointCheckingCour = PointCheckingCour(() =>
                {
                    StopGame();
                    SetGate(false);
                    OnEndGame.Invoke();
                    OnGameEnd.Invoke();
                });

                enemyCheckingCour = EnemyCheckingCour(() =>
                {
                    StopGame();
                    SetGate(false);
                    OnEndGame.Invoke();
                    OnGameEnd.Invoke();
                });

                StartCoroutine(gameTimerCour);
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

        StopCoroutine(gameTimerCour);
        StopCoroutine(pointCheckingCour);
        StopCoroutine(enemyCheckingCour);
        StopCoroutine(spawningCour);
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

    public void UpdateSpawningLanes()
    {
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

            yield return new WaitForSeconds(sessionData.enemySpawnInterval / sessionData.enemySpeed);
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
        alien.SetMovementSpeed(sessionData.enemySpeed * 0.7f);
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

    IEnumerator TimeCour(int timerDuration, TextMeshProUGUI txt, UnityAction OnEndTimer, bool inMinutes = false)
    {
        txt.gameObject.SetActive(true);
        int currentTime = timerDuration;
        while (currentTime >= 0)
        {
            int minutes = currentTime / 60;
            int seconds = currentTime - (minutes * 60);
            txt.text = inMinutes ? minutes + ":" + seconds :  currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        txt.gameObject.SetActive(false);
        OnEndTimer.Invoke();
    }

    #endregion

    #region Points

    IEnumerator PointCheckingCour(UnityAction OnEndGame)
    {
        yield return new WaitUntil(() => currentPoints >= sessionData.pointsToEarn);
        OnEndGame.Invoke();
    }

    private void AddPoint()
    {
        if(currentPoints + sessionData.pointPerEnemy <= sessionData.pointsToEarn)
        {
            currentPoints += sessionData.pointPerEnemy;
            txtPoints.text = currentPoints + "/" + sessionData.pointsToEarn;
            if(currentPoints == sessionData.pointsToEarn)
            {
                ShowGameResult(true);
            }
        }
    }
    IEnumerator EnemyCheckingCour(UnityAction OnEndGame)
    {
        yield return new WaitUntil(() => currentEnemyReachedCockpit >= sessionData.numberOfFails);
        OnEndGame.Invoke();
    }

    private void AddEnemyReachedCockpit()
    {
        currentEnemyReachedCockpit++;
        if (currentEnemyReachedCockpit == sessionData.numberOfFails)
        {
            ShowGameResult(false);
        }
    }

    private void ShowGameResult(bool success)
    {
        pnlHUD.SetActive(false);
        pnlGameResult.SetActive(true);
        txtEndPoints.text = txtPoints.text;
        txtEndTime.text = txtTimer.text;
        txtEndResult.text = success ? "Success" : "Failed";

        txtEndPoints.color = success ? txtEndPoints.color : colorFailedText;
        txtEndResult.color = success ? colorSuccessText : colorFailedText;

        if (success)
        {
            AssistantBehavior.Instance.Speak(gameSuccessClip);
            TrophyManager.Instance.isGame1Accomplished = true;
        }
        else
        {
            AssistantBehavior.Instance.Speak(gameFailClip);
        }

        if (!success) { return; }
        UserDataManager.Instance.AddStars(currentPoints);
    }

    #endregion

    #region Environment

    public void SetGate(bool open)
    {
        if (open)
        {
            leftGate.DOLocalMoveX(leftGate.transform.localPosition.x + 0.08f, 1f);
            rightGate.DOLocalMoveX(rightGate.transform.localPosition.x - 0.09f, 1f);
        }
        else
        {
            leftGate.DOLocalMoveX(leftGate.transform.localPosition.x - 0.08f, 1f);
            rightGate.DOLocalMoveX(rightGate.transform.localPosition.x + 0.09f, 1f);
        }
    }

    #endregion

    #region Set Player

    public void SetPlayerSettings(bool snapPlayer)
    {
        isPlayerLocked = snapPlayer;
        CharacterManager.Instance.VRRig.GetComponent<VRRig>().IsStationary = snapPlayer;

        if (snapPlayer)
        {
            CharacterManager.Instance.VRFootIK.GetComponent<VRFootIK>().PlaceLegOnBox();
        }
        else
        {
            CharacterManager.Instance.VRFootIK.GetComponent<VRFootIK>().UnPlaceAllLegOnBox();
        }
    }

    public void WaitUntilPlayerIsLocked(UnityAction OnReady)
    {
        StartCoroutine(WaitUntilPlayerIsLockedCour(OnReady));
    }

    IEnumerator WaitUntilPlayerIsLockedCour(UnityAction OnEnd)
    {
        while (!isPlayerLocked)
        {
            yield return new WaitForEndOfFrame();
        }
        OnEnd.Invoke();
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        foreach (var spawnPoint in enemySpawnPoints)
        {
            Gizmos.DrawSphere(spawnPoint.point.position, 1f);
        }
    }

    #endregion

    #region Enable Start Button

    public void EnableStartButton()
    {
        if (!isBowlingGameInstructionDone)
        {
            pnlStartGame.gameObject.SetActive(true);
        }
    }

    #endregion
}

public class BowlingSessionData : SessionData
{
    public List<Side> lanes = new List<Side>() { Side.Left, Side.Middle, Side.Right };
    public float enemySpeed = 1f;
    public float enemySpawnInterval = 5f;
    public int pointsToEarn = 100;
    public int numberOfFails = 10;
    public int pointPerEnemy = 5;
    public int timeDuration = 180;
    public float dispenserOffset = 0.5f;
}