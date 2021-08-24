using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Wait Until Action", menuName = "Action/Wait Until", order = 5)]
public class WaitUntilPlayerIsReadyAction : Action
{
    public GameID game = GameID.Bowling;

    public override void ExecuteAction(TutorialActor actor, UnityAction OnEndAction)
    {
        switch (game)
        {
            case GameID.Bowling:
                BowlingGameManagement.Instance.WaitUntilPlayerIsLocked(OnEndAction);
                break;
            case GameID.Game2:
                break;
            case GameID.Game3:
                break;
            case GameID.AvatarRoom:
                break;
            default:
                break;
        }
    }
}
