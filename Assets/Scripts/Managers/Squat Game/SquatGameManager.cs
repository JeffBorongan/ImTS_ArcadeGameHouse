using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.AI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class SquatGameManager : GameManagement
{
    #region Singleton

    public static SquatGameManager Instance { private set; get; }

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

    public SquatGameSessionData sessionData = null;

    [Header("Enemy Spawning")]
    [SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> enemyDestinationPoints = new List<Transform>();
    private IEnumerator spawningCour = null;
    private bool isSpawning = false;
    private bool proceedToNextSpawn = false;

    [Header("Doors and Lights")]
    [SerializeField] private List<GameObject> doors = new List<GameObject>();
    [SerializeField] private List<GameObject> blockers = new List<GameObject>();
    [SerializeField] private List<MeshRenderer> lights = new List<MeshRenderer>();
    [SerializeField] private Texture2D redLightBaseMap;
    [SerializeField] private Texture2D redLightEmissionMap;
    [SerializeField] private Texture2D greenLightBaseMap;
    [SerializeField] private Texture2D greenLightEmissionMap;
    private MoveStatus moveStatus = MoveStatus.None;
    private bool isDoorMovable = false;

    [Header("Levers")]
    [SerializeField] private GameObject leftHandle;
    [SerializeField] private GameObject rightHandle;

    [Header("UI")]
    [SerializeField] private GameObject pnlHUD = null;
    [SerializeField] private GameObject pnlGameResult = null;
    [SerializeField] private TextMeshProUGUI txtCountdownTimer = null;
    [SerializeField] private TextMeshProUGUI txtEndResult = null;
    [SerializeField] private Color colorSuccessText = Color.blue;
    [SerializeField] private Color colorFailedText = Color.blue;
    private IEnumerator countdownTimerCour = null;

    [SerializeField] private AudioClip gameSuccessClip = null;
    private AlienMovement alien = null;
    private IEnumerator enemyCheckingCour = null;
    private int currentEnemyReachedTheDoor = 0;
    private int index = 0;
    private int blockerIndex = 0;
    [SerializeField] private GameObject closeDoorDetection = null;

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
        sessionData = (SquatGameSessionData)data;
        btnStartGame.onClick.RemoveAllListeners();
        btnStartGame.onClick.AddListener(() =>
        {
            UXManager.Instance.HandleOnSquatGameStart();
            CharacterManager.Instance.PointersVisibility(false);
            countdownTimerCour = TimeCour(3, txtCountdownTimer, () =>
            {
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
            door.transform.DOMoveY(sessionData.doorFullCloseDistance, sessionData.doorMoveSpeed);
        }

        StopCoroutine(enemyCheckingCour);
        StopCoroutine(spawningCour);
    }

    #endregion

    #region Game Update

    private void Update()
    {
        if (sessionData != null)
        {
            UpdateLeverPulling(isSpawning, sessionData.pullUpHeight, sessionData.pushDownHeight);
        }
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
        alien.AlienAgent.SetDestination(enemyGoal.position);

        alien.OnDeath.AddListener(() =>
        {
            alien.OnDeath.RemoveAllListeners();
        });

        alien.OnReachDestination.AddListener(() =>
        {
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

            for (int i = index; i < 5; i++)
            {
                lights[i].materials[sessionData.doorFrameLightMaterialIndex].SetTexture("_BaseMap", redLightBaseMap);
                lights[i].materials[sessionData.doorFrameLightMaterialIndex].SetTexture("_EmissionMap", redLightEmissionMap);
            }

            ShowGameResult(false);

            OnGameEnd.Invoke();
        }
    }

    #endregion

    #region Lever Pulling

    private void UpdateLeverPulling(bool engageLever, float pullUpHeight, float pushDownHeight)
    {
        if (leftHandle.GetComponent<XRGrabInteractable>().isSelected && rightHandle.GetComponent<XRGrabInteractable>().isSelected)
        {
            leftHandle.GetComponent<XRGrabInteractable>().trackPosition = true;

            if (engageLever)
            {
                if (leftHandle.transform.position.y <= pushDownHeight && rightHandle.transform.position.y <= pushDownHeight)
                {
                    if (moveStatus == MoveStatus.Half)
                    {
                        isDoorMovable = true;
                        doors[index].transform.DOMoveY(sessionData.doorHalfCloseDistance, sessionData.doorMoveSpeed);
                        blockers[blockerIndex].GetComponent<NavMeshObstacle>().enabled = false;
                    }
                    if (moveStatus == MoveStatus.Whole)
                    {
                        isDoorMovable = false;
                        doors[index].GetComponent<NavMeshObstacle>().enabled = true;
                        blockers[++blockerIndex].GetComponent<NavMeshObstacle>().enabled = false;
                        lights[index].materials[sessionData.doorFrameLightMaterialIndex].SetTexture("_BaseMap", greenLightBaseMap);
                        lights[index].materials[sessionData.doorFrameLightMaterialIndex].SetTexture("_EmissionMap", greenLightEmissionMap);
                        doors[index].transform.DOMoveY(sessionData.doorFullCloseDistance, sessionData.doorMoveSpeed).OnComplete(() =>
                        {
                            if (isSpawning && index == 4)
                            {
                                StopGame();
                                ShowGameResult(true);
                                OnGameEnd.Invoke();
                            }
                            else
                            {
                                index++;
                                blockerIndex++;
                            }

                            proceedToNextSpawn = true;
                        });
                        moveStatus = MoveStatus.None;
                    }
                }

                if (leftHandle.transform.position.y >= pullUpHeight && rightHandle.transform.position.y >= pullUpHeight)
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
        else
        {
            leftHandle.GetComponent<XRGrabInteractable>().trackPosition = false;
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

        if (success)
        {
            AssistantBehavior.Instance.Speak(gameSuccessClip);
            AssistantBehavior.Instance.PlayCelebratingAnimation();
            TrophyManager.Instance.AddGameAccomplished((int)GameNumber.Game2);
        }

        VoiceOverManager.Instance.ButtonsInteraction(true, false, false, false);
        ElevatorManager.Instance.CloseDoorDetection = true;
    }

    #endregion

    #region Enable Start Button

    public void EnableStartButton()
    {
        pnlStartGame.gameObject.SetActive(true);
        CharacterManager.Instance.PointersVisibility(true);
    }

    #endregion

    #region Close Door Detection

    private void OnTriggerEnter(Collider other)
    {
        other = closeDoorDetection.GetComponent<Collider>();
        if (other.CompareTag("Head"))
        {
            CharacterManager.Instance.PointersVisibility(false);
            ElevatorManager.Instance.CloseElevatorDoor();
            closeDoorDetection.SetActive(false);
        }
    }

    #endregion
}

public class SquatGameSessionData : SessionData
{
    public int enemyReachedTheDoor = 1;
    public float enemySpeed = 1f;

    public float pullUpHeight = 1f;
    public float pushDownHeight = 0.5f;

    public int doorFrameLightMaterialIndex = 1;
    public float doorHalfCloseDistance = 0f;
    public float doorFullCloseDistance = 0.955f;
    public float doorFullOpenDistance = -0.955f;
    public float doorMoveSpeed = 0.5f;
}

public enum MoveStatus
{
    None,
    Half,
    Whole,
}