using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AssistantBehavior : MonoBehaviour
{
    public static AssistantBehavior Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private Transform player = null;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float maxUpLocation;
    [SerializeField] private float maxDownLocation;
    [SerializeField] private float originalLocation;
    private bool isReadyToLoop = true;
    private NavMeshAgent agent = null;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = CharacterManager.Instance.VRCamera.transform;
    }

    private void Update()
    {
        if (isReadyToLoop)
        {
            StartCoroutine(FloatingMovement());
        }
    }

    public void Speak(AudioClip clip)
    {
        transform.DOLookAt(player.position, 0.2f, AxisConstraint.Y, Vector3.up).OnComplete(() =>
        {
            audioSource.clip = clip;
            audioSource.Play();
        });
    }

    IEnumerator FloatingMovement()
    {
        isReadyToLoop = false;
        transform.DOMoveY(maxUpLocation, 3f).SetEase(Ease.Linear).OnComplete(() => transform.DOMoveY(maxDownLocation, 6f).SetEase(Ease.Linear).OnComplete(() => transform.DOMoveY(originalLocation, 3f).SetEase(Ease.Linear).OnComplete(() => isReadyToLoop = true)));
        yield return new WaitUntil(() => isReadyToLoop);
    }
}