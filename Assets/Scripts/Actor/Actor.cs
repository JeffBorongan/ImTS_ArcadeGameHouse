using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    public UnityEvent OnEndAction = new UnityEvent();

    public void ExecuteAction(Action action)
    {

    }

    public void MoveTo(Transform targetPosition)
    {

    }

    public void Speak(AudioClip clip)
    {

    }
}
