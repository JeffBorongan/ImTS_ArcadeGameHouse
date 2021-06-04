using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public IntActionEvent OnCountDownStart = new IntActionEvent();
    public LevelActionEvent OnGameStart = new LevelActionEvent();
    public LevelActionEvent OnGameUpdate = new LevelActionEvent();
    public BoolEvent OnGameEnd = new BoolEvent();

    public IntEvent OnPointsUpdate = new IntEvent();

    public Level CurrentLevel { get => currentLevel; }

    public void StartGame(Level level)
    {
        OnCountDownStart.Invoke(5, () => 
        {
            currentLevel = level;
            currentLevel.StartLevel();
            OnGameStart.Invoke(currentLevel, HandleOnTimerEnd);
        });
    }

    public void StopGame()
    {
        currentLevel.StopLevel();
    }

    public void UpdateLevel(Level newLevel)
    {
        currentLevel.StopLevel();
        currentLevel = newLevel;
        OnGameUpdate.Invoke(currentLevel, HandleOnTimerEnd);
        currentLevel.StartLevel();
    }

    private void HandleOnTimerEnd()
    {
        GameEnd(currentPoint >= currentLevel.pointsToEarn);
    }

    public void AddAlienReachedCockpit()
    {
        currentAlienInTheCockpit++;

        if(currentAlienInTheCockpit >= currentLevel.aliensReachedCockpit)
        {
            GameEnd(false);
        }
    }

    public void AddPoint()
    {
        currentPoint += currentLevel.pointsPerAlien;
        OnPointsUpdate.Invoke(currentPoint);

        if(currentPoint >= currentLevel.pointsToEarn)
        {
            GameEnd(true);
        }
    }

    void GameEnd(bool win)
    {
        OnGameEnd.Invoke(win);
        currentLevel.StopLevel();
    }

    void SetDefault()
    {
        currentAlienInTheCockpit = 0;
        currentPoint = 0;
    }
}

public class LevelActionEvent : UnityEvent<Level, UnityAction> { }

public class IntActionEvent : UnityEvent<int, UnityAction> { }

public class IntEvent : UnityEvent<int> { }
