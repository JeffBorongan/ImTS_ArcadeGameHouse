using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public static PlayerHUD Instance { private set; get; }

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

    [Header("Message")]
    public Transform cameraPosition = null;
    public Transform HUDParent = null;
    public GameObject pnlHUDCanvas = null;
    public TextMeshProUGUI txtHUDMessage = null;

    public void ShowMessage(Vector3 direction, string message, bool show)
    {
        if (show)
        {
            pnlHUDCanvas.gameObject.SetActive(true);
            HUDParent.transform.localRotation = Quaternion.LookRotation(direction);
            HUDParent.localPosition = cameraPosition.localPosition;
            txtHUDMessage.text = message;
        }
        else
        {
            pnlHUDCanvas.gameObject.SetActive(false);
        }
    }
}
