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

    #endregion

    #region Startup

    private void Start()
    {
        XRSettings.gameViewRenderMode = GameViewRenderMode.OcclusionMesh;
        BowlingGameStart();
        SquatGameStart();
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
        pointsToEarn.onValueChanged.AddListener(HandleOnChangeTextField);
        numberOfFails.onValueChanged.AddListener(HandleOnChangeTextField);
        dispenserOffset.onValueChanged.AddListener(HandleOnChangeTextField);

        btnBowlingGameUpdate.onClick.AddListener(HandleOnBowlingGameUpdate);

        SetBowlingGameDefaultValues();
    }

    private void SquatGameStart()
    {
        btnSquatGameUpdate.onClick.AddListener(HandleOnSquatGameUpdate);

        pullUpHeight.onValueChanged.AddListener(HandleOnChangePullUpHeight);
        pushDownHeight.onValueChanged.AddListener(HandleOnChangePushDownHeight);

        SetSquatGameDefaultValues();
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

    private void HandleOnChangeTextField(string value)
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

    #endregion

    #region Handle Game Update

    private void HandleOnBowlingGameUpdate()
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
        BowlingGameManagement.Instance.UpdateDispensers(CharacterManager.Instance.CurrentAnatomy);
        BowlingGameManagement.Instance.UpdateSpawningLanes();

        btnBowlingGameUpdate.interactable = false;
    }

    private void HandleOnSquatGameUpdate()
    {
        pullUpHeightValue = pullUpHeight.value;
        pushDownHeightValue = pushDownHeight.value;

        pullUpHeightValue = Mathf.Round(pullUpHeightValue * 100f) / 100f;
        pushDownHeightValue = Mathf.Round(pushDownHeightValue * 100f) / 100f;

        SquatGameManager.Instance.SessionData.pullUpHeight = pullUpHeightValue;
        SquatGameManager.Instance.SessionData.pushDownHeight = pushDownHeightValue;

        btnSquatGameUpdate.interactable = false;
    }

    #endregion
}