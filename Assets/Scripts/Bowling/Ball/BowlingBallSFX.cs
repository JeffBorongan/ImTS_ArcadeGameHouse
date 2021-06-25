using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBallSFX : MonoBehaviour
{
    [SerializeField] private AudioClip onReleaseSFX = null;
    [SerializeField] private AudioClip onPickedUpSFX = null;
    [SerializeField] private AudioClip onRollingSFX = null;

    [SerializeField] private AudioSource onReleaseSource = null;
    [SerializeField] private AudioSource onPickedUpSource = null;
    [SerializeField] private AudioSource onRollingSource = null;

    private BowlingBall bowlingBall = null;

    private void Start()
    {
        bowlingBall = GetComponent<BowlingBall>();

        onRollingSource.clip = onRollingSFX;
        onRollingSource.loop = true;

        onReleaseSource.clip = onReleaseSFX;
        onPickedUpSource.clip = onPickedUpSFX;

        bowlingBall.OnPickedUp.AddListener(HanldeOnPickedUp);
        bowlingBall.OnRelease.AddListener(HandleOnRelease);
        bowlingBall.OnRolling.AddListener(HandleOnRolling);
    }

    private void HandleOnRolling(bool isRolling)
    {
        if (isRolling)
        {
            if (!onRollingSource.isPlaying)
            {
                onRollingSource.Play();
            }
        }
        else
        {
            if (onRollingSource.isPlaying)
            {
                onRollingSource.Stop();
            }
        }
    }

    private void HandleOnRelease()
    {
        onReleaseSource.Play();
    }

    private void HanldeOnPickedUp()
    {
        onPickedUpSource.Play();
    }
}
