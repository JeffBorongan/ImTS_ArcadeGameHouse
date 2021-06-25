using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager Instance { private set; get; }

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

    [SerializeField] private Actor currentActor = null;
    [SerializeField] private List<Action> listOfActions = new List<Action>();
    private int currentActionExecuted = 0;

    private void Start()
    {
        currentActor.OnEndAction.AddListener(HandleOnEndAction);
        StartCutScene();
    }

    private void HandleOnEndAction()
    {
        if (currentActionExecuted + 1 < listOfActions.Count) { currentActionExecuted++; }
        else { return; }

        StartCoroutine(CutsceneCour(listOfActions[currentActionExecuted]));
    }

    public void StartCutScene()
    {
        StartCoroutine(CutsceneCour(listOfActions[currentActionExecuted]));
    }

    IEnumerator CutsceneCour(Action action)
    {
        yield return new WaitForSeconds(action.delayTime);
        ExecuteAction(action);
    }

    void ExecuteAction(Action action)
    {
        currentActor.ExecuteAction(action);
    }
}