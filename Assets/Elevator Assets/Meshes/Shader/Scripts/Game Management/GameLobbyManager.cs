using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameLobbyManager : MonoBehaviour
{
    public static GameLobbyManager Instance { private set; get; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    private Dictionary<GameID, GameManagement> gameManagers = new Dictionary<GameID, GameManagement>();
    public Dictionary<GameID, GameManagement> GameManagers { get => gameManagers; }

    public void AddGame(GameID id, GameManagement game)
    {
        gameManagers.Add(id, game);
    }

    public void LoadGame(GameID id, bool load, UnityAction OnEnd)
    {
        if (load)
        {
            StartCoroutine(LoadGameCour(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)id + 1, LoadSceneMode.Additive), OnEnd));
        }
        else
        {
            StartCoroutine(LoadGameCour(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)id + 1), OnEnd));
        }
    }

    IEnumerator LoadGameCour(AsyncOperation operation, UnityAction OnEnd)
    {
        while (!operation.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        OnEnd.Invoke();
    }

}
