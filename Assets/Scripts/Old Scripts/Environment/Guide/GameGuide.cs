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
        EnvironmentGuideManager.Instance.StartCoroutine(DelayCour(() =>
        {
            switch (typeOfGame)
            {
                case GameGuideType.Tutorial:
                    GameLobbyManager.Instance.GameManagers[whichGame].StartTutorial(OnEndGuide);
                    break;
                case GameGuideType.Game:
                    GameLobbyManager.Instance.GameManagers[whichGame].StartGame(new BowlingSessionData(), OnEndGuide);
                    break;
                default:
                    break;
            }

            base.ShowGuide(OnEndGuide);
        }));
    }
}

public enum GameGuideType
{
    Tutorial,
    Game
}