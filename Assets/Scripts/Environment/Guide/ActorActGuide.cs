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
        actorAction.ExecuteAction(EnvironmentGuideManager.Instance.Actor, OnEndGuide);
    }

    public override void UnShowGuide()
    {

    }

    public override bool isGuideAcomplish()
    {
        return base.isGuideAcomplish();
    }
}
