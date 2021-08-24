using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Teleport Action", menuName = "Action/Teleport", order = 0)]
public class TeleportAction : Action
{
    [SerializeField] private EnvironmentPoints teleportPoint = EnvironmentPoints.GameLobbyCenter;

    public override void ExecuteAction(TutorialActor actor, UnityAction OnEndAction)
    {
        EnvironmentPoint newPoint = Environment.Instance.PointsDictionary[teleportPoint];
        actor.Teleport(newPoint.point.position);
        OnEndAction.Invoke();
    }
}
