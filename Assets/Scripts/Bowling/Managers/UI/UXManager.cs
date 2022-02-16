using DG.Tweening;
using System;
using System.Collections;
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

    [Header("Control Panel")]
    [SerializeField] private Transform controlPanel = null;
    [SerializeField] private Button btnCollapseControlPanel = null;
    [SerializeField] private Button btnUncollapseControlPanel = null;
    private Vector3 controlPanelStartPos = Vector3.zero;

    [Header("Spawning")]
    [SerializeField] private TMP_InputField enemySpawnInterval = null;

    [SerializeField] private Button btnLeftLaneChecked = null;
    [SerializeField] private Button btnLeftLaneUnchecked = null;

    [SerializeField] private Button btnMiddleLaneChecked = null;
    [SerializeField] private Button btnMiddleLaneUnchecked = null;

    [SerializeField] private Button btnRightLaneChecked = null;
    [SerializeField] private Button btnRightLaneUnchecked = null;

    private List<Side> currentLanesEnabled = new List<Side>();

    [Header("Alien")]
    [SerializeField] private TMP_InputField enemySpeed = null;

    [Header("Goal")]
    [SerializeField] private TMP_InputField pointsToEarn = null;
    [SerializeField] private TMP_InputField numberOfFails = null;

    [Header("Player")]
    [SerializeField] private TMP_InputField dispenserOffset = null;

    [Header("Update")]
    [SerializeField] private Button btnUpdate = null;

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

        enemySpawnInterval.onValueChanged.AddListener(HandleOnChange);
        enemySpeed.onValueChanged.AddListener(HandleOnChange);
        pointsToEarn.onValueChanged.AddListener(HandleOnChange);
        numberOfFails.onValueChanged.AddListener(HandleOnChange);
        dispenserOffset.onValueChanged.AddListener(HandleOnChange);

        btnCollapseControlPanel.onClick.AddListener(() => { CollapseControlPanel(true); });
        btnUncollapseControlPanel.onClick.AddListener(() => { CollapseControlPanel(false); });

        controlPanelStartPos = controlPanel.position;

        SetUIWithSessionData();
    }

    void SetUIWithSessionData()
    {
        SpaceBowlingSaveData localData = null;

        if (!LocalSavingManager.Instance.IsLocalDataStored("Space"))
        {
            SpaceBowlingSaveData defaultData = new SpaceBowlingSaveData();
            defaultData.enemySpawnIntervalValue = 3f;
            defaultData.enemySpeedValue = 1f;
            defaultData.pointsToEarnValue = 100;
            defaultData.numberOfFailsValue = 2;
            defaultData.dispenserOffsetValue = 0.5f;

            defaultData.lanes = "0,1,2,";

            localData = defaultData;
        }
        else
        {
            localData = LocalSavingManager.Instance.GetLocalData<SpaceBowlingSaveData>("Space");
        }

        enemySpawnInterval.text = localData.enemySpawnIntervalValue.ToString();
        enemySpeed.text = localData.enemySpeedValue.ToString();
        pointsToEarn.text = localData.pointsToEarnValue.ToString();
        numberOfFails.text = localData.numberOfFailsValue.ToString();
        dispenserOffset.text = localData.dispenserOffsetValue.ToString();

        string[] rawLanes = localData.lanes.Split(',');

        foreach (var lane in rawLanes)
        {
            if (lane != "")
            {
                Side newLane = (Side)int.Parse(lane);

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

    void SaveSessionData()
    {
        SpaceBowlingSaveData newData = new SpaceBowlingSaveData();

        newData.dataID = "Space";
        newData.enemySpawnIntervalValue = float.Parse(enemySpawnInterval.text);
        newData.enemySpeedValue = float.Parse(enemySpeed.text);
        newData.pointsToEarnValue = int.Parse(pointsToEarn.text);
        newData.numberOfFailsValue = int.Parse(numberOfFails.text);
        newData.dispenserOffsetValue = float.Parse(dispenserOffset.text);

        foreach (var lane in currentLanesEnabled)
        {
            newData.lanes += (int)lane + ",";
        }

        LocalSavingManager.Instance.SaveLocalData(newData);
    }

    private void CollapseControlPanel(bool collapse)
    {
        if (collapse)
        {
            controlPanel.DOScale(0, 0.3f);
            controlPanel.DOMove(btnCollapseControlPanel.transform.position, 0.3f).OnComplete(() =>
            {
                controlPanel.gameObject.SetActive(false);
            });
        }
        else
        {
            controlPanel.gameObject.SetActive(true);
            controlPanel.DOScale(1, 0.3f);
            controlPanel.DOMove(controlPanelStartPos, 0.3f);
        }
    }

    private void HandleOnChange(string value)
    {
        btnUpdate.interactable = true;
    }

    private void HandleOnUpdate()
    {
        float enemySpawnIntervalValue = float.Parse(enemySpawnInterval.text);
        float enemySpeedValue = float.Parse(enemySpeed.text);
        int pointsToEarnValue = int.Parse(pointsToEarn.text);
        int numberOfFailsValue = int.Parse(numberOfFails.text);
        float dispenserOffsetValue = float.Parse(dispenserOffset.text);

        BowlingGameManagement.Instance.sessionData.enemySpawnInterval = enemySpawnIntervalValue;
        BowlingGameManagement.Instance.sessionData.enemySpeed = enemySpeedValue;
        BowlingGameManagement.Instance.sessionData.pointsToEarn = pointsToEarnValue;
        BowlingGameManagement.Instance.sessionData.numberOfFails = numberOfFailsValue;
        BowlingGameManagement.Instance.sessionData.dispenserOffset = dispenserOffsetValue;
        BowlingGameManagement.Instance.sessionData.lanes = currentLanesEnabled;

        BowlingGameManagement.Instance.UpdateDispensers(BodyMeasurement.Instance.CurrentAnatomy);
        BowlingGameManagement.Instance.UpdateSpawningLanes();

        btnUpdate.interactable = false;

        SaveSessionData();
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

    public void HandleOnStart()
    {
        float enemySpawnIntervalValue = float.Parse(enemySpawnInterval.text);
        float enemySpeedValue = float.Parse(enemySpeed.text);
        int pointsToEarnValue = int.Parse(pointsToEarn.text);
        int numberOfFailsValue = int.Parse(numberOfFails.text);
        float dispenserOffsetValue = float.Parse(dispenserOffset.text);

        BowlingGameManagement.Instance.sessionData.enemySpawnInterval = enemySpawnIntervalValue;
        BowlingGameManagement.Instance.sessionData.enemySpeed = enemySpeedValue;
        BowlingGameManagement.Instance.sessionData.pointsToEarn = pointsToEarnValue;
        BowlingGameManagement.Instance.sessionData.numberOfFails = numberOfFailsValue;
        BowlingGameManagement.Instance.sessionData.dispenserOffset = dispenserOffsetValue;
        BowlingGameManagement.Instance.sessionData.lanes = currentLanesEnabled;

        SaveSessionData();
    }
}