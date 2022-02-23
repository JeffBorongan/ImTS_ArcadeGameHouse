using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class UXManager : MonoBehaviour
{
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

    [Header("Lanes")]
    [SerializeField] private Button btnLeftLaneChecked = null;
    [SerializeField] private Button btnLeftLaneUnchecked = null;
    [SerializeField] private Button btnMiddleLaneChecked = null;
    [SerializeField] private Button btnMiddleLaneUnchecked = null;
    [SerializeField] private Button btnRightLaneChecked = null;
    [SerializeField] private Button btnRightLaneUnchecked = null;
    private List<Side> currentLanesEnabled = new List<Side>();

    [Header("Parameters")]
    [SerializeField] private Slider enemySpawnInterval = null;
    [SerializeField] private TMP_Text txtEnemySpawnInterval = null;
    [SerializeField] private Slider enemySpeed = null;
    [SerializeField] private TMP_Text txtEnemySpeed = null;
    [SerializeField] private TMP_InputField dispenserOffset = null;
    [SerializeField] private TMP_InputField pointsToEarn = null;
    [SerializeField] private TMP_InputField numberOfFails = null;

    [SerializeField] private Button btnUpdate = null;

    private float enemySpawnIntervalValue = 5f;
    private float enemySpeedValue = 1f;
    private int pointsToEarnValue = 100;
    private int numberOfFailsValue = 2;
    private float dispenserOffsetValue = 0.5f;

    private void Start()
    {
        XRSettings.gameViewRenderMode = GameViewRenderMode.OcclusionMesh;

        btnLeftLaneChecked.onClick.AddListener(() => { SetLanes(Side.Left, false); });
        btnLeftLaneUnchecked.onClick.AddListener(() => { SetLanes(Side.Left, true); });

        btnMiddleLaneChecked.onClick.AddListener(() => { SetLanes(Side.Middle, false); });
        btnMiddleLaneUnchecked.onClick.AddListener(() => { SetLanes(Side.Middle, true); });

        btnRightLaneChecked.onClick.AddListener(() => { SetLanes(Side.Right, false); });
        btnRightLaneUnchecked.onClick.AddListener(() => { SetLanes(Side.Right, true); });

        btnUpdate.onClick.AddListener(HandleOnUpdate);

        enemySpawnInterval.onValueChanged.AddListener(HandleOnChangeEnemySpawnInterval);
        enemySpeed.onValueChanged.AddListener(HandleOnChangeEnemySpeed);
        pointsToEarn.onValueChanged.AddListener(HandleOnChangeTextField);
        numberOfFails.onValueChanged.AddListener(HandleOnChangeTextField);
        dispenserOffset.onValueChanged.AddListener(HandleOnChangeTextField);

        SetUIWithDefaultValues();
    }

    private void SetLanes(Side side, bool add)
    {
        if (add)
        {
            currentLanesEnabled.Add(side);
        }
        else
        {
            currentLanesEnabled.Remove(side);
        }

        btnUpdate.interactable = true;
    }

    private void SetUIWithDefaultValues()
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

    private void HandleOnChangeEnemySpawnInterval(float value)
    {
        value = Mathf.Round(value * 100f) / 100f;
        txtEnemySpawnInterval.text = value.ToString();
        btnUpdate.interactable = true;
    }

    private void HandleOnChangeEnemySpeed(float value)
    {
        value = Mathf.Round(value * 100f) / 100f;
        txtEnemySpeed.text = value.ToString();
        btnUpdate.interactable = true;
    }

    private void HandleOnChangeTextField(string value)
    {
        btnUpdate.interactable = true;
    }

    public void HandleOnStart()
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

    private void HandleOnUpdate()
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

        BowlingGameManagement.Instance.UpdateDispensers(BodyMeasurement.Instance.CurrentAnatomy);
        BowlingGameManagement.Instance.UpdateSpawningLanes();

        btnUpdate.interactable = false;
    }
}