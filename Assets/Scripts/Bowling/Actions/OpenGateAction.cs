using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Open Gate Action", menuName = "Action/Open Gate", order = 3)]
public class OpenGateAction : Action
{
    public bool openTheGate = true;
    public override void ExecuteAction(TutorialActor actor, UnityAction OnEndAction)
    {
        if (openTheGate)
        {
            ShipManager.Instance.OpenDoor();
        }
        else
        {
            ShipManager.Instance.CloseDoor();
        }
        OnEndAction.Invoke();
    }
}
