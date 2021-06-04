using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance { private set; get; }

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

    [SerializeField] private Transform leftDoor = null;
    [SerializeField] private Transform rightDoor = null;
    private float leftDoorPositionX = 0f;
    private float rightDoorPositionX = 0f;

    private void Start()
    {
        //leftDoorPositionX = leftDoor.position.x;
        //rightDoorPositionX = rightDoor.position.x;

        GameManager.Instance.OnGameEnd.AddListener(HandleOnGameEnd);
        GameManager.Instance.OnGameStart.AddListener(HandleOnGameStart);
    }

    private void HandleOnGameStart(Level level, UnityAction callback)
    {
        OpenDoor();
    }

    private void HandleOnGameEnd(bool win)
    {
        CloseDoor();
    }

    public void OpenDoor()
    {
        leftDoor.DOLocalMoveX(7.34f, 1f);
        rightDoor.DOLocalMoveX(-0.329f, 1f);
    }

    public void CloseDoor()
    {
        leftDoor.DOLocalMoveX(3.568514f, 1f);
        rightDoor.DOLocalMoveX(3.568514f, 1f);
    }
}
