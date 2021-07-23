using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Action : ScriptableObject
{
    public float delayTime = 0;
    public virtual void ExecuteAction(TutorialActor actor, UnityAction OnEndAction) { }

}

public enum ActionType
{
    None,
    Move,
    Speak
}