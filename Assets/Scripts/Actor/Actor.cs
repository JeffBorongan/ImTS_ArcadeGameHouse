using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    AudioSource audioSource;
    public UnityEvent OnEndAction = new UnityEvent();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ExecuteAction(Action action)
    {
        switch (action.type())
        {
            case ActionType.Move:
                MoveToLocation(((MoveAction)action).targetPosition, 3f, Ease.Linear);
                break;
            case ActionType.Speak:
                Speak(((SpeakAction)action).clip);
                break;
            default:
                break;
        }
    }

    public void MoveToLocation(Transform targetPosition, float travelDuration, Ease easeType)
    {
        transform.DOMove(targetPosition.position, travelDuration).SetEase(easeType).OnComplete(OnEndAction.Invoke);
    }

    public void Speak(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(InvokeEndAudioClipCour());
    }

    IEnumerator InvokeEndAudioClipCour()
    {
        while (audioSource.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }

        OnEndAction.Invoke();
    }
}