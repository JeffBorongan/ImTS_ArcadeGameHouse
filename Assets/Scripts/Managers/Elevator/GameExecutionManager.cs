using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameExecutionManager : MonoBehaviour
{
    #region Singleton

    public static GameExecutionManager Instance { private set; get; }

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

    #endregion

    #region Parameters

    public UnityEvent OnInitialize = new UnityEvent();
    public UnityEvent OnEventRegistration = new UnityEvent();

    #endregion

    #region Execution

    private void Start()
    {
        StartCoroutine(ExecuteCour());
    }

    private IEnumerator ExecuteCour()
    {
        yield return new WaitForEndOfFrame();
        OnEventRegistration.Invoke();
        yield return new WaitForEndOfFrame();
        OnInitialize.Invoke();
    }

    #endregion
}