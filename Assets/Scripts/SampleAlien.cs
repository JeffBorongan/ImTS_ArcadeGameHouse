using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SampleAlien : MonoBehaviour
{
    public UnityEvent OnDeath = new UnityEvent();

    private void Start()
    {
        transform.DOMove(transform.position + transform.forward * 10f, 3f).OnComplete(HandleOnComplete);
    }

    private void HandleOnComplete()
    {
        OnDeath.Invoke();
        Destroy(gameObject);
    }
}
