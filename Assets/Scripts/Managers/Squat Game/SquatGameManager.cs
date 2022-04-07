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

    [Header("Enemy")]
    [SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> enemyDestinationPoints = new List<Transform>();
    private AlienMovement alien = null;
    private IEnumerator spawningCour = null;
    private bool isSpawning = false;
    private bool proceedToNextSpawn = false;

    [Header("Doors and Lights")]
    [SerializeField] private List<GameObject> doors = new List<GameObject>();
    [SerializeField] private List<GameObject> barriers = new List<GameObject>();
    [SerializeField] private List<MeshRenderer> lights = new List<MeshRenderer>();
    [SerializeField] private Texture2D redLightBaseMapOn = null;
    [SerializeField] private Texture2D redLightBaseMapOff = null;
    [SerializeField] private Texture2D redLightEmissionMapOn = null;
    [SerializeField] private Texture2D redLightEmissionMapOff = null;
    [SerializeField] private Texture2D greenLightBaseMap = null;
    [SerializeField] private Texture2D greenLightEmissionMap = null;
    private IEnumerator doorBlinkingCour = null;
    private DoorStatus doorStatus = DoorStatus.None;
    private bool isDoorMovable = false;
    private int barrierIndex = 0;

    [Header("Handles")]
    [SerializeField] private GameObject leftHandle = null;
    [SerializeField] private GameObject rightHandle = null;

    [Header("UI")]
    [SerializeField] private GameObject videoDemo = null;
    [SerializeField] private GameObject pnlHUD = null;
    [SerializeField] private GameObject pnlGameResult = null;
    [SerializeField] private TextMeshProUGUI txtCountdownTimer = null;
    [SerializeField] private TextMeshProUGUI txtEndResult = null;
    [SerializeField] private Color colorSuccessText = Color.blue;
    private IEnumerator countdownTimerCour = null;

    [Space, Space, Space]
    [SerializeField] private GameObject vFXConfetti = null;
    [SerializeField] private AudioClip gameSuccessClip = null;
    [SerializeField] private List<GameObject> disableObjects = new List<GameObject>();
    private SquatGameSessionData sessionData = null;
    private int index = 0;

    #endregion

    #region Encapsulations

    public GameObject VideoDemo { get => videoDemo; }
    public SquatGameSessionData SessionData { get => sessionData; set => sessionData = value; }
    public List<GameObject> DisableObjects { get => disableObjects; }

    #endregion

    #region Initialize

    public override void InitializeGame()
    {
        StartGame(new SquatGameSessionData(), () => { });
    }

    #endregion

    #region Start

    public override void StartGame(SessionData data, UnityAction OnEndGame)
    {
        SessionData = (SquatGameSessionData)data;
        btnStartGame.onClick.RemoveAllListeners();
        btnStartGame.onClick.AddListener(() =>
        {
            UXManager.Instance.HandleOnSquatGameStart();
            CharacterManager.Instance.PointersVisibility(false);
            countdownTimerCour = TimeCour(3, txtCountdownTimer, () =>
            {
                VoiceOverManager.Instance.LastButtonSelected = LastButtonSelected.Game2Controls;
                VoiceOverManager.Instance.InvokeLastButtonSelected();
                isSpawning = true;
                spawningCour = SpawningCour();
                StartCoroutine(spawningCour);
            });

            StartCoroutine(countdownTimerCour);
            pnlStartGame.gameObject.SetActive(false);
        });
    }

    #endregion

    #region Update

    private void Update()
    {
        if (SessionData != null)
        {
            StartCoroutine(LeverMechanics(isSpawning, SessionData.pullUpHeight, SessionData.pushDownHeight));
        }
    }

    #endregion

    #region Lever Mechanics

    private IEnumerator LeverMechanics(bool engageLever, float pullUpHeight, float pushDownHeight)
    {
        yield return new WaitUntil(() => 
            leftHandle.GetComponent<XRGrabInteractable>().isSelected && rightHandle.GetComponent<XRGrabInteractable>().isSelected ? 
            leftHandle.GetComponent<XRGrabInteractable>().trackPosition = true : 
            leftHandle.GetComponent<XRGrabInteractable>().trackPosition = false
        );

        if (engageLever)
        {
            if (leftHandle.transform.position.y <= pushDownHeight && rightHandle.transform.position.y <= pushDownHeight)
            {
                if (doorStatus == DoorStatus.Half)
                {
                    isDoorMovable = true;
                    doors[index].transform.DOMoveY(SessionData.doorHalfCloseDistance, SessionData.doorMoveSpeed);
                    barriers[barrierIndex].GetComponent<NavMeshObstacle>().enabled = false;
                }

                if (doorStatus == DoorStatus.Whole)
                {
                    isDoorMovable = false;
                    doors[index].GetComponent<NavMeshObstacle>().enabled = true;
                    barriers[++barrierIndex].GetComponent<NavMeshObstacle>().enabled = false;
                    StopCoroutine(doorBlinkingCour);
                    lights[index].materials[SessionData.doorFrameLightMaterialIndex].SetTexture("_BaseMap", greenLightBaseMap);
                    lights[index].materials[SessionData.doorFrameLightMaterialIndex].SetTexture("_EmissionMap", greenLightEmissionMap);
                    doors[index].transform.DOMoveY(SessionData.doorFullCloseDistance, SessionData.doorMoveSpeed).OnComplete(() =>
                    {
                        if (isSpawning && index == 4)
                        {
                            isSpawning = false;
                            StopCoroutine(spawningCour);
                            ShowGameResult();
                            OnGameEnd.Invoke();
                        }
                        else
                        {
                            index++;
                            barrierIndex++;
                            proceedToNextSpawn = true;
                        }
                    });

                    doorStatus = DoorStatus.None;
                }
            }

            if (leftHandle.transform.position.y >= pullUpHeight && rightHandle.transform.position.y >= pullUpHeight)
            {
                if (!isDoorMovable)
                {
                    doorStatus = DoorStatus.Half;
                }

                if (isDoorMovable)
                {
                    doorStatus = DoorStatus.Whole;
                }
            }
        }
    }

    #endregion

    #region Door Light Blinking

    private IEnumerator DoorBlinking(int number, float waitTime)
    {
        while (true)
        {
            lights[number].materials[SessionData.doorFrameLightMaterialIndex].SetTexture("_BaseMap", redLightBaseMapOn);
            lights[number].materials[SessionData.doorFrameLightMaterialIndex].SetTexture("_EmissionMap", redLightEmissionMapOn);
            yield return new WaitForSeconds(waitTime);
            lights[number].materials[SessionData.doorFrameLightMaterialIndex].SetTexture("_BaseMap", redLightBaseMapOff);
            lights[number].materials[SessionData.doorFrameLightMaterialIndex].SetTexture("_EmissionMap", redLightEmissionMapOff);
            yield return new WaitForSeconds(waitTime);
        }
    }

    #endregion

    #region Enemy Spawning

    private IEnumerator SpawningCour()
    {
        while (isSpawning)
        {
            SpawnEnemy(enemySpawnPoints[index], enemyDestinationPoints[index]);
            doorBlinkingCour = DoorBlinking(index, 1f);
            StartCoroutine(doorBlinkingCour);
            yield return new WaitUntil(() => proceedToNextSpawn);
        }
    }

    private void SpawnEnemy(Transform spawnPoint, Transform enemyGoal)
    {
        proceedToNextSpawn = false;
        GameObject clone = ObjectPoolingManager.Instance.GetFromPool(TypeOfObject.Alien2);
        clone.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        clone.SetActive(true);
        alien = clone.GetComponent<AlienMovement>();
        alien.SetMovementSpeed(SessionData.enemySpeed);
        alien.AlienAgent.SetDestination(enemyGoal.position);
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

    private void ShowGameResult()
    {
        pnlHUD.SetActive(false);
        pnlGameResult.SetActive(true);
        txtEndResult.text = "Success";
        txtEndResult.color = colorSuccessText;
        vFXConfetti.SetActive(true);
        VoiceOverManager.Instance.ButtonsInteraction(false);
        AssistantBehavior.Instance.Move(true, 3f, () =>
        {
            AssistantBehavior.Instance.Speak(gameSuccessClip);
            AssistantBehavior.Instance.PlayCelebratingAnimation();
            AssistantBehavior.Instance.Animator.SetBool("isBlinking", true);
            StartCoroutine(FunctionWithDelay(gameSuccessClip.length, () => 
            {
                vFXConfetti.SetActive(false);
                AssistantBehavior.Instance.Animator.SetBool("isBlinking", false);
                TrophyManager.Instance.AddGameAccomplished((int)GameNumber.Game2);
                VoiceOverManager.Instance.LastButtonSelected = LastButtonSelected.LockEmUpToElevator;
                VoiceOverManager.Instance.InvokeLastButtonSelected();
                ElevatorManager.Instance.CloseDoorDetection = true;
            }));
        });
    }

    #endregion

    #region Enable Start Button

    public void EnableStartButton()
    {
        pnlStartGame.gameObject.SetActive(true);
        CharacterManager.Instance.PointersVisibility(true);
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

public class SquatGameSessionData : SessionData
{
    public float enemySpeed = 1f;

    public int doorFrameLightMaterialIndex = 1;
    public float doorHalfCloseDistance = 0f;
    public float doorFullCloseDistance = 0.955f;
    public float doorFullOpenDistance = -0.955f;
    public float doorMoveSpeed = 0.5f;

    public float pullUpHeight = 1f;
    public float pushDownHeight = 0.5f;
}