using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Guide : ScriptableObject
{
    [SerializeField] private bool enableRoomInteraction = true;
    [SerializeField] private float delayTime = 0f;

    public virtual void ShowGuide(UnityAction OnEndGuide) 
    {
        UserInteraction.Instance.SetRoomInteraction(enableRoomInteraction);
    }
    public virtual void UnShowGuide() { }

    public virtual bool isGuideAcomplish() { return false; }

    protected IEnumerator DelayCour(UnityAction OnDelay)
    {
        yield return new WaitForSeconds(delayTime);
        OnDelay.Invoke();
    }
}
