using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance { private set; get; }

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

    public UnityEvent OnGameStart = new UnityEvent();
    public UnityEvent OnGameStop = new UnityEvent();
    public UnityEvent OnGameReset = new UnityEvent();

    public virtual void StartGame() { }
    public virtual void StopGame() { }
    public virtual void ResetGame() { }
}
