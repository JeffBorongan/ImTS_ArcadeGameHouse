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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private Transform player = null;
    [SerializeField] private AudioSource audioSource;
    private NavMeshAgent agent = null;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = ElevatorFloorManager.Instance.characterCamera.transform;
    }

    public void Speak(AudioClip clip)
    {
        transform.DOLookAt(player.position, 0.2f, AxisConstraint.Y, Vector3.up).OnComplete(() =>
        {
            audioSource.clip = clip;
            audioSource.Play();
        });
    }
}