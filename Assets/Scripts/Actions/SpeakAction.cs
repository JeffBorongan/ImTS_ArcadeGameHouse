using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Speak Action", menuName = "Action/Speak", order = 1)]
public class SpeakAction : Action
{
    public AudioClip clip = null;
}
