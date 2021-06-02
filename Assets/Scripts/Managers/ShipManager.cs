using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private Transform door = null;
    private float doorPositionX = 0f;

    private void Start()
    {
        doorPositionX = door.position.x;
    }

    public void OpenDoor()
    {
        door.DOLocalMoveX(2, 0.2f);
    }

    public void CloseDoor()
    {
        door.DOLocalMoveX(-5, 0.2f);
    }
}
