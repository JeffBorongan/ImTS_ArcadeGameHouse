using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : ScriptableObject
{
    public virtual void ShowGuide() { }
    public virtual void UnShowGuide() { }

    public virtual bool isGuideAcomplish() { return false; }
}
