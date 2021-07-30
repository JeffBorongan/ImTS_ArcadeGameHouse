using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Game Guide", menuName = "Guide/Game Guide", order = 3)]
public class GameGuide : Guide
{
    [SerializeField] private GameID whichGame = GameID.Bowling;
    [SerializeField] public GameGuideType typeOfGame = GameGuideType.Tutorial;
    public override void ShowGuide(UnityAction OnEndGuide)
    {
        switch (typeOfGame)
        {
            case GameGuideType.Tutorial:
                Environment.Instance.GameManagers[whichGame].StartTutorial(OnEndGuide);
                break;
            case GameGuideType.Game:
                Environment.Instance.GameManagers[whichGame].StartGame(new BowlingSessionData(), OnEndGuide);
                break;
            default:
                break;
        }

        base.ShowGuide(OnEndGuide);
    }
}

public enum GameGuideType
{
    Tutorial,
    Game
}