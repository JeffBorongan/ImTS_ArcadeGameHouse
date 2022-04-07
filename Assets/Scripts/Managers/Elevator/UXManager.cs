using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class UXManager : MonoBehaviour
{
    #region Singleton

    public static UXManager Instance { private set; get; }

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

    [Header("Bowling Game")]
    [SerializeField] private Button btnLeftLaneChecked = null;
    [SerializeField] private Button btnLeftLaneUnchecked = null;
    [SerializeField] private Button btnMiddleLaneChecked = null;
    [SerializeField] private Button btnMiddleLaneUnchecked = null;
    [SerializeField] private Button btnRightLaneChecked = null;
    [SerializeField] private Button btnRightLaneUnchecked = null;
    [SerializeField] private Slider enemySpawnInterval = null;
    [SerializeField] private TMP_Text txtEnemySpawnInterval = null;
    [SerializeField] private Slider enemySpeed = null;
    [SerializeField] private TMP_Text txtEnemySpeed = null;
    [SerializeField] private TMP_InputField dispenserOffset = null;
    [SerializeField] private TMP_InputField pointsToEarn = null;
    [SerializeField] private TMP_InputField numberOfFails = null;
    [SerializeField] private Button btnBowlingGameUpdate = null;
    private List<Side> currentLanesEnabled = new List<Side>();
    private float enemySpawnIntervalValue = 5f;
    private float enemySpeedValue = 1f;
    private int pointsToEarnValue = 100;
    private int numberOfFailsValue = 10;
    private float dispenserOffsetValue = 0.5f;

    [Header("Squat Game")]
    [SerializeField] private Slider pullUpHeight = null;
    [SerializeField] private TMP_Text txtPullUpHeight = null;
    [SerializeField] private Slider pushDownHeight = null;
    [SerializeField] private TMP_Text txtPushDownHeight = null;
    [SerializeField] private Button btnSquatGameUpdate = null;
    private float pullUpHeightValue = 1f;
    private float pushDownHeightValue = 0.5f;

    [Header("Whack Game")]
    [SerializeField] private Button btnPlayerSpeedIncrease = null;
    [SerializeField] private Button btnPlayerSpeedDecrease = null;
    [SerializeField] private TMP_Text txtPlayerSpeed = null;
    [SerializeField] private TMP_InputField aliensToHit = null;
    [SerializeField] private TMP_InputField enemyReachedThePlayer = null;
    [SerializeField] private Button btnWhackGameUpdate = null;
    private float playerSpeedValue = 0.2f;
    private float playerSpeedLowestValue = 0.2f;
    private float playerSpeedHighestValue = 8f;
    private float playerSpeedChange = 0.1f;
    private int aliensToHitValue = 10;
    private int enemyReachedThePlayerValue = 5;

    #endregion

    #region Startup

    private void Start()
    {
        XRSettings.gameViewRenderMode = GameViewRenderMode.OcclusionMesh;
        BowlingGameStart();
        SquatGameStart();
        WhackGameStart();
    }

    private void BowlingGameStart()
    {
        btnLeftLaneChecked.onClick.AddListener(() => { SetBowlingLanes(Side.Left, false); });
        btnLeftLaneUnchecked.onClick.AddListener(() => { SetBowlingLanes(Side.Left, true); });
        btnMiddleLaneChecked.onClick.AddListener(() => { SetBowlingLanes(Side.Middle, false); });
        btnMiddleLaneUnchecked.onClick.AddListener(() => { SetBowlingLanes(Side.Middle, true); });
        btnRightLaneChecked.onClick.AddListener(() => { SetBowlingLanes(Side.Right, false); });
        btnRightLaneUnchecked.onClick.AddListener(() => { SetBowlingLanes(Side.Right, true); });

        enemySpawnInterval.onValueChanged.AddListener(HandleOnChangeEnemySpawnInterval);
        enemySpeed.onValueChanged.AddListener(HandleOnChangeEnemySpeed);
        pointsToEarn.onValueChanged.AddListener(HandleOnChangeBowlingTextField);
        numberOfFails.onValueChanged.AddListener(HandleOnChangeBowlingTextField);
        dispenserOffset.onValueChanged.AddListener(HandleOnChangeBowlingTextField);

        btnBowlingGameUpdate.onClick.AddListener(() => 
        {
            HandleOnBowlingGameStart();
            BowlingGameManagement.Instance.UpdateDispensers(CharacterManager.Instance.CurrentAnatomy);
            BowlingGameManagement.Instance.UpdateSpawningLanes();
            btnBowlingGameUpdate.interactable = false;
        });

        SetBowlingGameDefaultValues();
    }

    private void SquatGameStart()
    {
        pullUpHeight.onValueChanged.AddListener(HandleOnChangePullUpHeight);
        pushDownHeight.onValueChanged.AddListener(HandleOnChangePushDownHeight);

        btnSquatGameUpdate.onClick.AddListener(() =>
        {
            HandleOnSquatGameStart();
            btnSquatGameUpdate.interactable = false;
        });

        SetSquatGameDefaultValues();
    }

    private void WhackGameStart()
    {
        btnPlayerSpeedIncrease.onClick.AddListener(() => HandleOnIncreasePlayerSpeed());
        btnPlayerSpeedDecrease.onClick.AddListener(() => HandleOnDecreasePlayerSpeed());
        aliensToHit.onValueChanged.AddListener(HandleOnChangeWhackTextField);
        enemyReachedThePlayer.onValueChanged.AddListener(HandleOnChangeWhackTextField);

        btnWhackGameUpdate.onClick.AddListener(() =>
        {
            HandleOnWhackGameStart();
            btnWhackGameUpdate.interactable = false;
        });

        SetWhackGameDefaultValues();
    }

    #endregion

    #region Default Values

    private void SetBowlingLanes(Side side, bool add)
    {
        if (add)
        {
            currentLanesEnabled.Add(side);
        }
        else
        {
            currentLanesEnabled.Remove(side);
        }

        btnBowlingGameUpdate.interactable = true;
    }

    private void SetBowlingGameDefaultValues()
    {
        enemySpawnIntervalValue = Mathf.Round(enemySpawnIntervalValue * 100f) / 100f;
        enemySpeedValue = Mathf.Round(enemySpeedValue * 100f) / 100f;
        dispenserOffsetValue = Mathf.Round(dispenserOffsetValue * 100f) / 100f;

        enemySpawnInterval.value = enemySpawnIntervalValue;
        enemySpeed.value = enemySpeedValue;
        pointsToEarn.text = pointsToEarnValue.ToString();
        numberOfFails.text = numberOfFailsValue.ToString();
        dispenserOffset.text = dispenserOffsetValue.ToString();

        List<Side> lanes = new List<Side>() { Side.Left, Side.Middle, Side.Right };

        foreach (var lane in lanes)
        {
            if (lane != Side.None)
            {
                Side newLane = lane;

                switch (newLane)
                {
                    case Side.Left:
                        btnLeftLaneUnchecked.onClick.Invoke();
                        break;
                    case Side.Middle:
                        btnMiddleLaneUnchecked.onClick.Invoke();
                        break;
                    case Side.Right:
                        btnRightLaneUnchecked.onClick.Invoke();
                        break;
                    case Side.Count:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void SetSquatGameDefaultValues()
    {
        pullUpHeightValue = Mathf.Round(pullUpHeightValue * 100f) / 100f;
        pushDownHeightValue = Mathf.Round(pushDownHeightValue * 100f) / 100f;

        pullUpHeight.value = pullUpHeightValue;
        pushDownHeight.value = pushDownHeightValue;
    }

    private void SetWhackGameDefaultValues()
    {
        playerSpeedValue = Mathf.Round(playerSpeedValue * 100f) / 100f;
        txtPlayerSpeed.text = playerSpeedValue.ToString();
        aliensToHit.text = aliensToHitValue.ToString();
        enemyReachedThePlayer.text = enemyReachedThePlayerValue.ToString();
    }

    #endregion

    #region Handle Value Changes

    private void HandleOnChangeEnemySpawnInterval(float value)
    {
        value = Mathf.Round(value * 100f) / 100f;
        txtEnemySpawnInterval.text = value.ToString();
        btnBowlingGameUpdate.interactable = true;
    }

    private void HandleOnChangeEnemySpeed(float value)
    {
        value = Mathf.Round(value * 100f) / 100f;
        txtEnemySpeed.text = value.ToString();
        btnBowlingGameUpdate.interactable = true;
    }

    private void HandleOnChangeBowlingTextField(string value)
    {
        btnBowlingGameUpdate.interactable = true;
    }

    private void HandleOnChangePullUpHeight(float value)
    {
        value = Mathf.Round(value * 100f) / 100f;
        txtPullUpHeight.text = value.ToString();
        btnSquatGameUpdate.interactable = true;
    }

    private void HandleOnChangePushDownHeight(float value)
    {
        value = Mathf.Round(value * 100f) / 100f;
        txtPushDownHeight.text = value.ToString();
        btnSquatGameUpdate.interactable = true;
    }

    private void HandleOnIncreasePlayerSpeed()
    {
        if (playerSpeedValue < playerSpeedHighestValue)
        {
            playerSpeedValue += playerSpeedChange;
            playerSpeedValue = Mathf.Round(playerSpeedValue * 100f) / 100f;
            txtPlayerSpeed.text = playerSpeedValue.ToString();
            WhackGameManager.Instance.SessionData.playerSpeed = playerSpeedValue;
        }
    }

    private void HandleOnDecreasePlayerSpeed()
    {
        if (playerSpeedValue > playerSpeedLowestValue)
        {
            playerSpeedValue -= playerSpeedChange;
            playerSpeedValue = Mathf.Round(playerSpeedValue * 100f) / 100f;
            txtPlayerSpeed.text = playerSpeedValue.ToString();
            WhackGameManager.Instance.SessionData.playerSpeed = playerSpeedValue;
        }
    }

    private void HandleOnChangeWhackTextField(string value)
    {
        btnWhackGameUpdate.interactable = true;
    }

    #endregion

    #region Handle Game Start

    public void HandleOnBowlingGameStart()
    {
        enemySpawnIntervalValue = enemySpawnInterval.value;
        enemySpeedValue = enemySpeed.value;
        pointsToEarnValue = int.Parse(pointsToEarn.text);
        numberOfFailsValue = int.Parse(numberOfFails.text);
        dispenserOffsetValue = float.Parse(dispenserOffset.text);

        enemySpawnIntervalValue = Mathf.Round(enemySpawnIntervalValue * 100f) / 100f;
        enemySpeedValue = Mathf.Round(enemySpeedValue * 100f) / 100f;
        dispenserOffsetValue = Mathf.Round(dispenserOffsetValue * 100f) / 100f;

        BowlingGameManagement.Instance.sessionData.enemySpawnInterval = enemySpawnIntervalValue;
        BowlingGameManagement.Instance.sessionData.enemySpeed = enemySpeedValue;
        BowlingGameManagement.Instance.sessionData.pointsToEarn = pointsToEarnValue;
        BowlingGameManagement.Instance.sessionData.numberOfFails = numberOfFailsValue;
        BowlingGameManagement.Instance.sessionData.dispenserOffset = dispenserOffsetValue;
        BowlingGameManagement.Instance.sessionData.lanes = currentLanesEnabled;
    }

    public void HandleOnSquatGameStart()
    {
        pullUpHeightValue = pullUpHeight.value;
        pushDownHeightValue = pushDownHeight.value;

        pullUpHeightValue = Mathf.Round(pullUpHeightValue * 100f) / 100f;
        pushDownHeightValue = Mathf.Round(pushDownHeightValue * 100f) / 100f;

        SquatGameManager.Instance.SessionData.pullUpHeight = pullUpHeightValue;
        SquatGameManager.Instance.SessionData.pushDownHeight = pushDownHeightValue;
    }

    public void HandleOnWhackGameStart()
    {
        aliensToHitValue = int.Parse(aliensToHit.text);
        enemyReachedThePlayerValue = int.Parse(enemyReachedThePlayer.text);
        WhackGameManager.Instance.SessionData.playerSpeed = playerSpeedValue;
        WhackGameManager.Instance.SessionData.aliensToHit = aliensToHitValue;
        WhackGameManager.Instance.SessionData.enemyReachedThePlayer = enemyReachedThePlayerValue;
    }

    #endregion
}