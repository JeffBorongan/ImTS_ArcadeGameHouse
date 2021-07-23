using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Speak Action", menuName = "Action/Speak", order = 1)]
public class SpeakAction : Action
{
    public AudioClip clip = null;

    public override void ExecuteAction(TutorialActor actor, UnityAction OnEndAction)
    {
        actor.Speak(clip, OnEndAction);
    }
}
