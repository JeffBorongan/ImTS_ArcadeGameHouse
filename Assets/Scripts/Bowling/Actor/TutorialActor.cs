using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using DG.Tweening;

public class TutorialActor : MonoBehaviour
{
    [SerializeField] private Transform player = null;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animator animator = null;
    private NavMeshAgent agent = null;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Speak(AudioClip clip, UnityAction OnEndAction)
    {
        transform.DOLookAt(player.position, 0.2f, AxisConstraint.Y, Vector3.up).OnComplete(() =>
        {
            ChangeAnimationState(ActorState.Speak);
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(InvokeEndAudioClipCour(clip.length, OnEndAction));
        });
    }

    IEnumerator InvokeEndAudioClipCour(float duration, UnityAction OnEndAction)
    {
        yield return new WaitForSeconds(duration);
        ChangeAnimationState(ActorState.Idle);
        OnEndAction.Invoke();
    }

    public void Move(Vector3 newPosition, float stopDistance, UnityAction OnEndAction)
    {
        ChangeAnimationState(ActorState.Walking);
        agent.stoppingDistance = stopDistance;
        agent.SetDestination(newPosition);
        StartCoroutine(MoveCour(OnEndAction));
    }

    IEnumerator MoveCour(UnityAction OnEndAction)
    {
        yield return new WaitUntil(() => agent.remainingDistance != 0);
        bool moving = true;
        while (moving)
        {
            if(agent.remainingDistance <= 0f)
            {
                ChangeAnimationState(ActorState.Idle);
                moving = false;
            }
            yield return new WaitForEndOfFrame();
        }
        OnEndAction.Invoke();
    }

    public void Teleport(Vector3 newPosition)
    {
        gameObject.SetActive(false);
        transform.position = newPosition;
        gameObject.SetActive(true);
    }

    public void ChangeAnimationState(ActorState state)
    {
        animator.SetTrigger("ChangeState");
        animator.SetInteger("State", (int)state);
    }
}

public enum ActorState
{
    Idle,
    Walking,
    Speak,
    Communicate
}
