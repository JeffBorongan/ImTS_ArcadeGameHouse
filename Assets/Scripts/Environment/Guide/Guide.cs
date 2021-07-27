using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Guide : ScriptableObject
{
    public virtual void ShowGuide(UnityAction OnEndGuide) { }
    public virtual void UnShowGuide() { }

    public virtual bool isGuideAcomplish() { return false; }
}
