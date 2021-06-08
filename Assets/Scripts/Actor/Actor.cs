using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animator animator = null;

    public UnityEvent OnEndAction = new UnityEvent();

    private ActorState currentState = ActorState.Idle;

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
        transform.DOLookAt(targetPosition.position, 0.2f, AxisConstraint.Y).OnComplete(() =>
        {
            ChangeAnimationState(ActorState.Walking);
            transform.DOMove(targetPosition.position, travelDuration).SetEase(easeType).OnComplete(() => {
                OnEndAction.Invoke();
                ChangeAnimationState(ActorState.Idle);
            });
        });
    }

    public void Speak(AudioClip clip)
    {
        ChangeAnimationState(ActorState.Talking);

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

        ChangeAnimationState(ActorState.Idle);
        OnEndAction.Invoke();
    }

    public void ChangeAnimationState(ActorState state)
    {
        if(currentState != state)
        {
            animator.SetTrigger("ChangeState");
            animator.SetInteger("State", (int)state);
            currentState = state;
        }
    }
}

public enum ActorState
{
    Idle,
    Walking,
    Talking
}