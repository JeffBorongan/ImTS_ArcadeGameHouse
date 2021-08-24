using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaTriggerDetection : MonoBehaviour
{
    [SerializeField] private string triggerTag = "";
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private UnityEvent OnTrigger = new UnityEvent();

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnce && triggered) { return; }

        if (other.CompareTag(triggerTag))
        {
            triggered = true;
            OnTrigger.Invoke();
        }
    }
}
