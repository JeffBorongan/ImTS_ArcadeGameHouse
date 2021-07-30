using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameExecution : MonoBehaviour
{
    public static GameExecution Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public UnityEvent OnInitialize = new UnityEvent();
    public UnityEvent OnEventRegistration = new UnityEvent();

    private void Start()
    {
        StartCoroutine(ExecuteCour());
    }

    IEnumerator ExecuteCour()
    {
        yield return new WaitForEndOfFrame();
        OnEventRegistration.Invoke();
        yield return new WaitForEndOfFrame();
        OnInitialize.Invoke();
    }

}
