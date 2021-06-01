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

    public void StartGame(Level level)
    {
        currentLevel = level;
        currentLevel.StartLevel();
    }

    public void StopGame()
    {
        currentLevel.StopLevel();
    }

    public void UpdateLevel(Level newLevel)
    {
        currentLevel.StopLevel();

        currentLevel = newLevel;
        currentLevel.StartLevel();
    }

    public void AddAlienReachedCockpit()
    {
        currentAlienInTheCockpit++;

        if(currentAlienInTheCockpit >= 1)
        {
            //Lose
        }
    }

    public void AddPoint()
    {
        currentPoint++;

        if(currentPoint >= currentLevel.pointsToEarn)
        {
            //Win
        }
    }

}
