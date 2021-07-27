using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterHelmet : MonoBehaviour
{
    public static CharacterHelmet Instance { private set; get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [SerializeField] private GameObject visorUI = null;

    public void OnSelect(ActivateEventArgs selectEvent)
    {
        if (visorUI.activeSelf)
        {
            visorUI.SetActive(false);
        }
        else
        {
            visorUI.SetActive(true);
        }
    }
}
