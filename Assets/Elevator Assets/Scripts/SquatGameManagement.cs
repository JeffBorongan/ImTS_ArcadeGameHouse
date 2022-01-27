using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;

public class SquatGameManagement : GameManagement
{
    #region Singleton

    public static SquatGameManagement Instance { private set; get; }

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

    private SquatGameSessionData sessionData = null;

    [Header("Enemy Spawning")]
    [SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> enemyDestinationPoints = new List<Transform>();
    private IEnumerator spawningCour = null;
    private bool isSpawning = false;
    private bool proceedToNextSpawn = false;

    [Header("Doors and Lights")]
    [SerializeField] private List<GameObject> doors = new List<GameObject>();
    [SerializeField] private List<GameObject> lights = new List<GameObject>();
    [SerializeField] private Material redLight;
    [SerializeField] private Material greenLight;
    private MoveStatus moveStatus = MoveStatus.None;
    private bool isDoorMovable = false;

    [Header("Levers")]
    [SerializeField] private GameObject leftLever;
    [SerializeField] private GameObject rightLever;

    [Header("UI")]
    [SerializeField] private Image imgTimerIcon = null;
    [SerializeField] private TextMeshProUGUI txtCountdownTimer = null;
    private IEnumerator countdownTimerCour = null;

    private AlienMovement alien = null;
    private IEnumerator enemyCheckingCour = null;
    private int currentEnemyReachedTheDoor = 0;
    private int index = 0;

    #endregion

    #region Initialize

    public override void InitializeGame()
    {
        StartGame(new SquatGameSessionData(), () => { });
    }

    #endregion

    #region Game Start

    public override void StartGame(SessionData data, UnityAction OnEndGame)
    {
        pnlStartGame.gameObject.SetActive(true);
        btnStartGame.onClick.RemoveAllListeners();
        btnStartGame.onClick.AddListener(() =>
        {
            sessionData = (SquatGameSessionData)data;
            countdownTimerCour = TimeCour(3, txtCountdownTimer, () =>
            {
                imgTimerIcon.gameObject.SetActive(true);

                foreach (var door in doors)
                {
                    door.transform.DOMoveY(-1.1f, 0.5f);
                }

                foreach (var light in lights)
                {
                    light.GetComponent<Renderer>().material = redLight;
                }

                isSpawning = true;
                spawningCour = SpawningCour();
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
        isSpawning = false;

        foreach (var door in doors)
        {
            door.transform.DOMoveY(1f, 0.5f);
        }

        foreach (var light in lights)
        {
            light.GetComponent<Renderer>().material = redLight;
        }

        StopCoroutine(enemyCheckingCour);
        StopCoroutine(spawningCour);
    }

    #endregion

    #region Game Update

    private void Update()
    {
        UpdateLeverPulling(isSpawning, sessionData.playerLeverLift, sessionData.playerLeverDrop);
    }

    #endregion

    #region Enemy Spawning

    IEnumerator SpawningCour()
    {
        while (isSpawning)
        {
            SpawnEnemy(enemySpawnPoints[index], enemyDestinationPoints[index]);
            yield return new WaitUntil(() => proceedToNextSpawn);
        }
    }

    private void SpawnEnemy(Transform spawnPoint, Transform enemyGoal)
    {
        proceedToNextSpawn = false;
        GameObject clone = ObjectPooling.Instance.GetFromPool(TypeOfObject.EnemyAlien);
        clone.transform.position = spawnPoint.position;
        clone.transform.rotation = spawnPoint.rotation;
        clone.SetActive(true);

        alien = clone.GetComponent<AlienMovement>();
        alien.SetMovementSpeed(sessionData.enemySpeed);
        Vector3 targetDestination = new Vector3(alien.transform.position.x, alien.transform.position.y, enemyGoal.position.z);
        alien.AlienAgent.SetDestination(targetDestination);

        alien.OnDeath.AddListener(() =>
        {
            alien.OnDeath.RemoveAllListeners();
        });

        alien.OnReachDestination.AddListener(() =>
        {
            AddEnemyReachedTheDoor();
            alien.OnReachDestination.RemoveAllListeners();
        });
    }

    #endregion

    #region Enemy Status Checking

    IEnumerator EnemyCheckingCour(UnityAction OnEndGame)
    {
        yield return new WaitUntil(() => currentEnemyReachedTheDoor >= sessionData.enemyReachedTheDoor);
        OnEndGame.Invoke();
    }

    private void AddEnemyReachedTheDoor()
    {
        currentEnemyReachedTheDoor++;
        if (currentEnemyReachedTheDoor == sessionData.enemyReachedTheDoor)
        {
            StopGame();
            OnGameEnd.Invoke();
        }
    }

    #endregion

    #region Lever Pulling

    private void UpdateLeverPulling(bool engageLever, float maximumLift, float minimumDrop)
    {
        leftLever.GetComponent<XRGrabInteractable>().enabled = engageLever;
        rightLever.GetComponent<XRGrabInteractable>().enabled = engageLever;

        if (engageLever)
        {
            if (leftLever.transform.position.y <= minimumDrop && rightLever.transform.position.y <= minimumDrop)
            {
                if (moveStatus == MoveStatus.Half)
                {
                    isDoorMovable = true;
                    doors[index].transform.DOMoveY(sessionData.doorHalfCloseDistance, sessionData.doorCloseSpeed);
                }
                if (moveStatus == MoveStatus.Whole)
                {
                    isDoorMovable = false;
                    doors[index].GetComponent<NavMeshObstacle>().enabled = true;
                    lights[index].GetComponent<Renderer>().material = greenLight;
                    doors[index].transform.DOMoveY(sessionData.doorFullCloseDistance, sessionData.doorCloseSpeed).OnComplete(() =>
                    {
                        if (isSpawning && index == 4)
                        {
                            StopGame();
                            OnGameEnd.Invoke();
                        }
                        else
                        {
                            index++;
                        }

                        proceedToNextSpawn = true;
                    });
                    moveStatus = MoveStatus.None;
                }
            }

            if (leftLever.transform.position.y >= maximumLift && rightLever.transform.position.y >= maximumLift)
            {
                if (!isDoorMovable)
                {
                    moveStatus = MoveStatus.Half;
                }
                if (isDoorMovable)
                {
                    moveStatus = MoveStatus.Whole;
                }
            }
        }
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
            txt.text = inMinutes ? minutes + ":" + seconds : currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        txt.gameObject.SetActive(false);
        OnEndTimer.Invoke();
    }

    #endregion
}

public class SquatGameSessionData : SessionData
{
    public float enemySpeed = 0.5f;
    public int enemyReachedTheDoor = 1;
    public float playerLeverLift = 1f;
    public float playerLeverDrop = 0.3f;
    public float doorHalfCloseDistance = 0f;
    public float doorFullCloseDistance = 1f;
    public float doorCloseSpeed = 0.5f;
}

public enum MoveStatus
{
    None,
    Half,
    Whole,
}