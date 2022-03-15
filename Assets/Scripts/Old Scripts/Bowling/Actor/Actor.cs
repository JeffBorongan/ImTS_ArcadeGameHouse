using DG.Tweening;
using System.Collections;
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
        ChangeAnimationState(ActorState.Communicate);

        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(InvokeEndAudioClipCour(clip.length));
    }

    IEnumerator InvokeEndAudioClipCour(float duration)
    {
        yield return new WaitForSeconds(duration);
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
