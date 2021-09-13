using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBodyPartCustom : MonoBehaviour
{
    public Image imgBodyPartCustom = null;
    public Button btnBodyPartCustom = null;
    public Image imgBodyPartPreview = null;
    public Image imgLocked = null;
    public GameObject currency = null;
    public TextMeshProUGUI txtStarCost = null;

    public UnityEvent OnHoverEnter = new UnityEvent();
    public UnityEvent OnHoverExit = new UnityEvent();

    public void OnHover(bool enter)
    {
        if (enter)
        {
            OnHoverEnter.Invoke();
        }
        else
        {
            OnHoverExit.Invoke();
        }
    }
}
