using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSFX : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] private AudioClip onDeathSFX = null;
    [SerializeField] private AudioClip onWalkingSFX = null;
    [SerializeField] private AudioClip onSpawnSFX = null;
    [SerializeField] private AudioClip onReachDestinationSFX = null;

    [Header("Source")]
    [SerializeField] private AudioSource onDeathSource = null;
    [SerializeField] private AudioSource onWalkingSource = null;
    [SerializeField] private AudioSource onSpawnSource = null;
    [SerializeField] private AudioSource onReachDestinationSource = null;

    private AlienMovement movement = null;

    private void Start()
    {
        movement = GetComponent<AlienMovement>();

        onSpawnSource.clip = onSpawnSFX;
        onWalkingSource.clip = onWalkingSFX;
        onReachDestinationSource.clip = onReachDestinationSFX;
        onDeathSource.clip = onDeathSFX;

        movement.OnDeath.AddListener(HandleOnDeath);
        movement.OnReachDestination.AddListener(HandleOnReachDestination);
        movement.OnSpawn.AddListener(HandleOnSpawn);
        movement.OnWalking.AddListener(HandleOnWalking);
    }

    private void HandleOnWalking()
    {
        if (onWalkingSource.isPlaying) { return; }
        onWalkingSource.loop = true;
        onWalkingSource.Play();
    }

    private void HandleOnSpawn()
    {        
        onSpawnSource.Play();
    }

    private void HandleOnReachDestination()
    {
        onReachDestinationSource.Play();
        onWalkingSource.Stop();
    }

    private void HandleOnDeath()
    {
        onDeathSource.Play();
        onWalkingSource.Stop();
    }
}
