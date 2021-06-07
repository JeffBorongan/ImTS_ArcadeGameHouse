using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Action : ScriptableObject
{
    public float delayTime = 0;

    public virtual ActionType type()
    {
        return ActionType.None;
    }

}

public enum ActionType
{
    None,
    Move,
    Speak
}