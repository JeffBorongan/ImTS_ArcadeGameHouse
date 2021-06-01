using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UXManager : MonoBehaviour
{
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
    [SerializeField] private TMP_InputField aliensReachedTheCockpit = null;

    [Header("Start")]
    [SerializeField] private Button btnStart = null;
    [SerializeField] private Button btnStop = null;

    [Header("Update")]
    [SerializeField] private Button btnUpdate = null;

    private void Start()
    {
        Display.displays[1].Activate();

        btnLeftLaneChecked.onClick.AddListener(() => { SetLanes(Side.Left, true); });
        btnLeftLaneUnchecked.onClick.AddListener(() => { SetLanes(Side.Left, false); });

        btnMiddleLaneChecked.onClick.AddListener(() => { SetLanes(Side.Middle, true); });
        btnMiddleLaneUnchecked.onClick.AddListener(() => { SetLanes(Side.Middle, false); });

        btnRightLaneChecked.onClick.AddListener(() => { SetLanes(Side.Right, true); });
        btnRightLaneUnchecked.onClick.AddListener(() => { SetLanes(Side.Right, false); });

        btnStart.onClick.AddListener(HandleOnStart);
        btnStop.onClick.AddListener(HandleOnStop);

        btnUpdate.onClick.AddListener(HandleOnUpdate);

        spawnTime.onValueChanged.AddListener(HandleOnChange);
        alienMovementSpeed.onValueChanged.AddListener(HandleOnChange);
        pointPerAlien.onValueChanged.AddListener(HandleOnChange);
        pointsToEarn.onValueChanged.AddListener(HandleOnChange);
        aliensReachedTheCockpit.onValueChanged.AddListener(HandleOnChange);
    }

    private void HandleOnChange(string value)
    {
        if(!GameManager.Instance.CurrentLevel.isStarted) { return; }

        btnUpdate.interactable = true;
    }

    private void HandleOnUpdate()
    {
        float spawnTimeValue = float.Parse(spawnTime.text);
        float alienMovementSpeedValue = float.Parse(alienMovementSpeed.text);
        float pointPerAlienValue = float.Parse(pointPerAlien.text);
        int pointsToEarnValue = int.Parse(pointsToEarn.text);
        float aliensReachedTheCockpitValue = float.Parse(aliensReachedTheCockpit.text);

        GameManager.Instance.UpdateLevel(new Level(alienMovementSpeedValue, spawnTimeValue, pointsToEarnValue, currentLanesEnabled, new List<GameObject>()));

        btnUpdate.interactable = false;
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
    }

    private void HandleOnStop()
    {
        GameManager.Instance.StopGame();
    }

    private void HandleOnStart()
    {
        float spawnTimeValue = float.Parse(spawnTime.text);
        float alienMovementSpeedValue = float.Parse(alienMovementSpeed.text);
        float pointPerAlienValue = float.Parse(pointPerAlien.text);
        int pointsToEarnValue = int.Parse(pointsToEarn.text);
        float aliensReachedTheCockpitValue = float.Parse(aliensReachedTheCockpit.text);

        GameManager.Instance.StartGame(new Level(alienMovementSpeedValue, spawnTimeValue, pointsToEarnValue, currentLanesEnabled, new List<GameObject>()));
    }
}
