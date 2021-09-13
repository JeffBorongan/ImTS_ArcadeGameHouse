using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public Transform pnlHUDCanvas = null;
    public TextMeshProUGUI txtHUDMessage = null;

    public void ShowMessage(Vector3 direction, string message, bool show)
    {
        if (show)
        {
            pnlHUDCanvas.gameObject.SetActive(true);
            pnlHUDCanvas.transform.rotation = Quaternion.LookRotation(direction);
            txtHUDMessage.text = message;
        }
        else
        {
            pnlHUDCanvas.gameObject.SetActive(false);
        }
    }
}
