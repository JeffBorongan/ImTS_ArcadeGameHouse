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
    [Header("Control Panel")]
    [SerializeField] private Transform controlPanel = null;
    [SerializeField] private Button btnCollapseControlPanel = null;
    [SerializeField] private Button btnUncollapseControlPanel = null;
    private Vector3 controlPanelStartPos = Vector3.zero;

    [Header("Spawning")]
    [SerializeField] private TMP_InputField spawnTime = null;

    [SerializeField] private Button btnLeftLaneChecked = null;
    [SerializeField] private Button btnLeftLaneUnchecked = null;

    [SerializeField] private Button btnMiddleLaneChecked = null;
    [SerializeField] private Button btnMiddleLaneUnchecked = null;

    [SerializeField] private Button btnRightLaneChecked = null;
    [SerializeField] private Button btnRightLaneUnchecked = null;

    private List<Side> currentLanesEnabled = new List<Side>();

    [Header("Alien")]
    [SerializeField] private TMP_InputField alienMovementSpeed = null;
    [SerializeField] private TMP_InputField pointPerAlien = null;

    [Header("Goal")]
    [SerializeField] private TMP_InputField pointsToEarn = null;
    [SerializeField] private TMP_InputField timeToBeat = null;
    [SerializeField] private TMP_InputField aliensReachedTheCockpit = null;

    [Header("Start")]
    [SerializeField] private Button btnStart = null;
    [SerializeField] private Button btnStop = null;

    [Header("Update")]
    [SerializeField] private Button btnUpdate = null;

    //[Header("Result")]
    //[SerializeField] private GameObject resultsPanel = null;

    private void Start()
    {
        XRSettings.gameViewRenderMode = GameViewRenderMode.OcclusionMesh;

        btnLeftLaneChecked.onClick.AddListener(() => { SetLanes(Side.Left, false); });
        btnLeftLaneUnchecked.onClick.AddListener(() => { SetLanes(Side.Left, true); });

        btnMiddleLaneChecked.onClick.AddListener(() => { SetLanes(Side.Middle, false); });
        btnMiddleLaneUnchecked.onClick.AddListener(() => { SetLanes(Side.Middle, true); });

        btnRightLaneChecked.onClick.AddListener(() => { SetLanes(Side.Right, false); });
        btnRightLaneUnchecked.onClick.AddListener(() => { SetLanes(Side.Right, true); });

        btnStart.onClick.AddListener(HandleOnStart);
        btnStop.onClick.AddListener(HandleOnStop);

        btnUpdate.onClick.AddListener(HandleOnUpdate);

        spawnTime.onValueChanged.AddListener(HandleOnChange);
        alienMovementSpeed.onValueChanged.AddListener(HandleOnChange);
        pointPerAlien.onValueChanged.AddListener(HandleOnChange);
        pointsToEarn.onValueChanged.AddListener(HandleOnChange);
        timeToBeat.onValueChanged.AddListener(HandleOnChange);
        aliensReachedTheCockpit.onValueChanged.AddListener(HandleOnChange);

        btnCollapseControlPanel.onClick.AddListener(() => { CollapseControlPanel(true); });
        btnUncollapseControlPanel.onClick.AddListener(() => { CollapseControlPanel(false); });

        controlPanelStartPos = controlPanel.position;

        GameManager.Instance.OnGameEnd.AddListener(HandleOnGameEnd);

        SetUIWithSessionData();
    }

    void SetUIWithSessionData()
    {
        if(!LocalSavingManager.Instance.IsLocalDataStored("Space")) { return; }

        SpaceBowlingSaveData localData = LocalSavingManager.Instance.GetLocalData<SpaceBowlingSaveData>("Space");

        spawnTime.text = localData.spawnTimeValue.ToString();
        alienMovementSpeed.text = localData.alienMovementSpeedValue.ToString();
        pointPerAlien.text = localData.pointPerAlienValue.ToString();
        pointsToEarn.text = localData.pointsToEarnValue.ToString();
        aliensReachedTheCockpit.text = localData.aliensReachedTheCockpitValue.ToString();
        timeToBeat.text = localData.timeToBeatValue.ToString();

        string[] rawLanes = localData.lanes.Split(',');

        foreach (var lane in rawLanes)
        {
            if(lane != "") 
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
        newData.spawnTimeValue = float.Parse(spawnTime.text);
        newData.alienMovementSpeedValue = float.Parse(alienMovementSpeed.text);
        newData.pointPerAlienValue = int.Parse(pointPerAlien.text);
        newData.pointsToEarnValue = int.Parse(pointsToEarn.text);
        newData.aliensReachedTheCockpitValue = int.Parse(aliensReachedTheCockpit.text);
        newData.timeToBeatValue = int.Parse(timeToBeat.text);

        foreach (var lane in currentLanesEnabled)
        {
            newData.lanes += (int)lane + ",";
        }

        LocalSavingManager.Instance.SaveLocalData(newData);
    }

    private void HandleOnGameEnd(bool win)
    {
        btnStart.gameObject.SetActive(true);
        btnStop.gameObject.SetActive(false);
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
        if(GameManager.Instance.CurrentLevel == null) { return; }

        if(!GameManager.Instance.CurrentLevel.isStarted) { return; }

        btnUpdate.interactable = true;
    }

    private void HandleOnUpdate()
    {
        float spawnTimeValue = float.Parse(spawnTime.text);
        float alienMovementSpeedValue = float.Parse(alienMovementSpeed.text);
        int pointPerAlienValue = int.Parse(pointPerAlien.text);
        int pointsToEarnValue = int.Parse(pointsToEarn.text);
        int aliensReachedTheCockpitValue = int.Parse(aliensReachedTheCockpit.text);
        int timeToBeatValue = int.Parse(timeToBeat.text);

        GameManager.Instance.UpdateLevel(new Level(alienMovementSpeedValue, spawnTimeValue, aliensReachedTheCockpitValue, pointPerAlienValue, pointsToEarnValue, timeToBeatValue, currentLanesEnabled, new List<GameObject>()));

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

        if (GameManager.Instance.CurrentLevel == null) { return; }

        if (!GameManager.Instance.CurrentLevel.isStarted) { return; }

        btnUpdate.interactable = true;
    }

    private void HandleOnStop()
    {
        GameManager.Instance.StopGame();
    }

    private void HandleOnStart()
    {
        float spawnTimeValue = float.Parse(spawnTime.text);
        float alienMovementSpeedValue = float.Parse(alienMovementSpeed.text);
        int pointPerAlienValue = int.Parse(pointPerAlien.text);
        int pointsToEarnValue = int.Parse(pointsToEarn.text);
        int aliensReachedTheCockpitValue = int.Parse(aliensReachedTheCockpit.text);
        int timeToBeatValue = int.Parse(timeToBeat.text);

        GameManager.Instance.StartGame(new Level(alienMovementSpeedValue, spawnTimeValue, aliensReachedTheCockpitValue, pointPerAlienValue, pointsToEarnValue, timeToBeatValue, currentLanesEnabled, new List<GameObject>()));

        SaveSessionData();
    }
}