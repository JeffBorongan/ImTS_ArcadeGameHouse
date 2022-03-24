using System.Collections.Generic;
using UnityEngine;

public class TrophyManager : MonoBehaviour
{
    #region Singleton

    public static TrophyManager Instance { private set; get; }

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

    private bool isGame1Failed = false;
    private List<int> gameAccomplished = new List<int>();
    private List<int> gameTrophyPresented = new List<int>();

    public void AddGameAccomplished(int gameNumber)
    {
        if (!IsGameAccomplished(gameNumber))
        {
            gameAccomplished.Add(gameNumber);
        }
    }

    public bool IsGameAccomplished(int gameNumber)
    {
        return gameAccomplished.Contains(gameNumber);
    }

    public void AddGameTrophyPresented(int gameNumber)
    {
        if (!IsGameTrophyPresented(gameNumber))
        {
            gameTrophyPresented.Add(gameNumber);
        }
    }

    public bool IsGameTrophyPresented(int gameNumber)
    {
        return gameTrophyPresented.Contains(gameNumber);
    }

    #endregion

    #region Encapsulations

    public bool IsGame1Failed { get => isGame1Failed; set => isGame1Failed = value; }

    #endregion
}