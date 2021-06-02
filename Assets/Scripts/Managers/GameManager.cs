using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private Level currentLevel = null;
    private int currentPoint = 0;
    private int currentAlienInTheCockpit = 0;
    public Level CurrentLevel { get => currentLevel; }

    private void Start()
    {
        
    }

    public void StartGame(Level level)
    {
        FloatingUIManager.Instance.StartCountdown(5, () =>
        {
            currentLevel = level;
            currentLevel.StartLevel();

            FloatingUIManager.Instance.StartTimer(TimeSpan.FromSeconds(currentLevel.timeToBeat), HandleOnTimerEnd);

        });
    }

    private void HandleOnTimerEnd()
    {
        if (currentPoint >= currentLevel.pointsToEarn)
        {
            FloatingUIManager.Instance.ShowGameResult(true);
        }
        else
        {
            FloatingUIManager.Instance.ShowGameResult(false);
        }
    }

    public void StopGame()
    {
        currentLevel.StopLevel();
    }

    public void UpdateLevel(Level newLevel)
    {
        currentLevel.StopLevel();
        FloatingUIManager.Instance.StopTimer(TimeSpan.FromSeconds(currentLevel.timeToBeat), HandleOnTimerEnd);

        currentLevel = newLevel;
        FloatingUIManager.Instance.StartTimer(TimeSpan.FromSeconds(currentLevel.timeToBeat), HandleOnTimerEnd);
        currentLevel.StartLevel();
    }

    public void AddAlienReachedCockpit()
    {
        currentAlienInTheCockpit++;

        if(currentAlienInTheCockpit >= 1)
        {
            FloatingUIManager.Instance.ShowGameResult(false);
        }
    }

    public void AddPoint()
    {
        currentPoint++;

        if(currentPoint >= currentLevel.pointsToEarn)
        {
            FloatingUIManager.Instance.ShowGameResult(true);
        }
    }

}
