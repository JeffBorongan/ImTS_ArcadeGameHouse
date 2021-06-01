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

}
