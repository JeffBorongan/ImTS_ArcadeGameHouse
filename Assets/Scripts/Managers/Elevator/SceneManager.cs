using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    #region Singleton

    public static SceneManager Instance { private set; get; }

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

    private Floors currentFloor = Floors.SpaceLobby;

    #endregion

    #region Scene Functions

    public void LoadFloor(Floors level, UnityAction OnComplete)
    {
        AsyncOperation unloadAsyncOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)currentFloor);
        StartCoroutine(LoadSceneCour(unloadAsyncOperation, () => 
        {
            AsyncOperation loadAsyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)level, LoadSceneMode.Additive);
            currentFloor = level;
            StartCoroutine(LoadSceneCour(loadAsyncOperation, OnComplete));
        }));
    }

    private IEnumerator LoadSceneCour(AsyncOperation operation, UnityAction OnComplete)
    {
        while (operation.progress < 1f )
        {
            yield return new WaitForEndOfFrame();
        }

        OnComplete.Invoke();
    }

    #endregion
}