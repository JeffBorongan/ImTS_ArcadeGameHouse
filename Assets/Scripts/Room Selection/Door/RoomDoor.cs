using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomDoor : Door
{
    public override void EnterRoom(UnityAction OnMid, UnityAction OnEnd)
    {
        ScreenFadeManager.Instance.FadeIn(() =>
        {
            UserInteraction.Instance.Teleport(Environment.Instance.PointsDictionary[parameters.destinationPoint].point.position);
            UserInteraction.Instance.SetCurrentRoom(parameters.room);

            if (OnMid != null)
            {
                OnMid.Invoke();
            }

            ScreenFadeManager.Instance.FadeOut(() =>
            {
                if (parameters.OnEnterDoor != null)
                {
                    parameters.OnEnterDoor.Invoke();
                }

                if(OnEnd != null)
                {
                    OnEnd.Invoke();
                }
            });
        });
    }
}
