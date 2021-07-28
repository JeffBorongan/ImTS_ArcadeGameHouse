using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance { private set; get; }

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

    //[SerializeField] private TextMeshProUGUI txtSubtitles = null;

    public void ShowSubtile(string message, float duration)
    {
        //txtSubtitles.text = message;
        //txtSubtitles.DOFade(1f, 0.2f).OnComplete(() =>
        //{
        //    txtSubtitles.DOFade(0f, 0.2f).SetDelay(duration);
        //});
    }

}
