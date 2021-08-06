using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Move Action", menuName = "Action/Move", order = 4)]
public class MoveAction : Action
{
    [SerializeField] private EnvironmentPoints point = EnvironmentPoints.Player;
    [SerializeField] private float stoppingDistance = 0f;

    public override void ExecuteAction(TutorialActor actor, UnityAction OnEndAction)
    {
        EnvironmentPoint newPoint = Environment.Instance.environmentPoints.Where(e => e.type == point).FirstOrDefault();
        actor.Move(newPoint.point.position, stoppingDistance, OnEndAction);
    }

}