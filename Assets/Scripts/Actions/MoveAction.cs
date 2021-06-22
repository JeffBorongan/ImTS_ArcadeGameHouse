using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move Action", menuName = "Action/Move", order = 2)]
public class MoveAction : Action
{
    public Transform targetPosition = null;
    public float travelDuration = 1f;

    public override ActionType type()
    {
        return ActionType.Move;
    }
}