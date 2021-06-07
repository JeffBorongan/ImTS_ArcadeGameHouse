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

    }

    public void MoveToLocation(Transform targetPosition, float travelDuration, Ease easeType)
    {
        transform.DOMove(targetPosition.position, travelDuration).SetEase(easeType).OnComplete(OnEndAction.Invoke);
    }

    public void Speak(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(InvokeOnEndClip(audioSource));
    }

    IEnumerator InvokeOnEndClip(AudioSource clip)
    { 
        yield return new WaitUntil(() => !clip.isPlaying);
        OnEndAction.Invoke();
    }
}