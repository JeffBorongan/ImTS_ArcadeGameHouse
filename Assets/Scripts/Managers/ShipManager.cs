using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [SerializeField] private Transform door = null;
    private float doorPositionX = 0f;

    private void Start()
    {
        doorPositionX = door.position.x;
    }

    public void OpenDoor()
    {
        door.DOLocalMoveX(0, 0.2f);
    }

    public void CloseDoor()
    {
        door.DOLocalMoveX(doorPositionX, 0.2f);
    }
}
