using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManagement : MonoBehaviour
{
    [Header("Tutorial")]
    public TutorialActor currentActor = null;
    public Transform player = null;
    public List<Tutorial> tutorialActions = new List<Tutorial>();
    private int currentTutorial = 0;

    [Header("Events")]
    public SessionDataEvent OnGameStart = new SessionDataEvent();

    public void StartTutorial(UnityAction OnEndTutorial)
    {
        ExecuteTutorial(currentActor, () => 
        {
            OnEndTutorial.Invoke();
        });
    }

    private void ExecuteTutorial(TutorialActor actor, UnityAction OnEndTutorial)
    {
        tutorialActions[currentTutorial].sequenceOfActions.StartSequence(actor, () => 
        {
            if (currentTutorial + 1 < tutorialActions.Count)
            {
                currentTutorial++;
                ExecuteTutorial(actor, OnEndTutorial);
            }
            else
            {
                OnEndTutorial.Invoke();
            }
        });
    }

    public void StopTutorial() { }

    public virtual void StartGame(SessionData data, UnityAction OnEndGame) { }
    public virtual void StopGame() { }
    public virtual void ResetGame() { }
}

[System.Serializable]
public class Tutorial
{
    public string tutorialName = "";
    public SequenceOfActions sequenceOfActions = new SequenceOfActions();
}

[System.Serializable]
public class SequenceOfActions
{
    public List<Action> actions = new List<Action>();
    public int currentSequenceIndex = 0;

    public void StartSequence(TutorialActor actor, UnityAction OnEndSequence)
    {
        ExecuteSequence(actor, OnEndSequence);
    }

    private void ExecuteSequence(TutorialActor actor, UnityAction OnEndSequence)
    {
        actions[currentSequenceIndex].ExecuteAction(actor, () =>
        {
            if (currentSequenceIndex + 1 < actions.Count)
            {
                currentSequenceIndex++;
                ExecuteSequence(actor, OnEndSequence);
            }
            else
            {
                OnEndSequence.Invoke();
            }
        });
    }
}

public class SessionData { }

[System.Serializable]
public class SessionDataEvent : UnityEvent<SessionData> { }