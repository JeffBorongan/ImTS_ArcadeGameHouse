using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Actor Guide", menuName = "Guide/Actor Guide", order = 1)]
public class ActorActGuide : Guide
{
    [SerializeField] private Action actorAction = null;

    public override void ShowGuide(UnityAction OnEndGuide)
    {
        EnvironmentGuideManager.Instance.StartCoroutine(DelayCour(() =>
        {
            actorAction.ExecuteAction(EnvironmentGuideManager.Instance.Actor, OnEndGuide);
            base.ShowGuide(OnEndGuide);
        }));
    }

    public override bool isGuideAcomplish()
    {
        return base.isGuideAcomplish();
    }
}
