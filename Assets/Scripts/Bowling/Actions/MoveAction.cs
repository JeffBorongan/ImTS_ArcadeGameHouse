using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Move Action", menuName = "Action/Move", order = 2)]
public class MoveAction : Action
{
    public int environmentPointIndex = 0;

    public override void ExecuteAction(TutorialActor actor, UnityAction OnEndAction)
    {
        actor.Move(Environment.Instance.points[environmentPointIndex].position, OnEndAction);
    }

}