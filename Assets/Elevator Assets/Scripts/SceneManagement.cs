using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    floor currentFloor = floor.SpaceLobby;


    public void LoadFloor(floor level, UnityAction OnComplete)
    {
        AsyncOperation unloadAsyncOperation = SceneManager.UnloadSceneAsync((int)currentFloor);
        StartCoroutine(LoadSceneCour(unloadAsyncOperation, () => {

            AsyncOperation loadAsyncOperation = SceneManager.LoadSceneAsync((int)level, LoadSceneMode.Additive);

            currentFloor = level;

            StartCoroutine(LoadSceneCour(loadAsyncOperation, OnComplete));
        }));
    }

    IEnumerator LoadSceneCour(AsyncOperation operation, UnityAction OnComplete)
    {
        while (operation.progress < 1f )
        {
            yield return new WaitForEndOfFrame();
        }

        OnComplete.Invoke();
    }
}
