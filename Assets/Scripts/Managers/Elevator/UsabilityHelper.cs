using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum UsabilityTimer
{ 
    Initiator,
    Lobby,
    Avatar,
    Elevator,
    Walkey,
    Lock,
    Bowling
}

public class UsabilityHelper : MonoBehaviour
{
    #region Singleton

    public static UsabilityHelper Instance { private set; get; }

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

    #endregion

    #region Parameters
    [SerializeField] private bool _initiator;

    [SerializeField] private GameObject _durationGO;

    [SerializeField] private List<TextMeshProUGUI> _texts;

    [SerializeField] private List<int> _durations;
    private int _timer;
    private UsabilityTimer _currentGame;
    #endregion

    private void Start()
    {
        if (_initiator)
        {
            StartUsabilityTimer(UsabilityTimer.Initiator);
        }
        else 
        {
            _durations[(int)UsabilityTimer.Initiator] = PlayerPrefs.GetInt("UsabilityInitiator");
        }

        if (_durationGO != null)
        {
            _durationGO.SetActive(false);
        }
    }

    private void OnApplicationQuit()
    {
        if (_initiator)
        {
            StopUsabilityTimer(UsabilityTimer.Initiator);
        }
    }

    public void StartUsabilityTimer(UsabilityTimer game)
    {
        _currentGame = game;

        if (_durations[(int)game] <= 0)
        {
            _timer = 0;
            StartCoroutine(CO_CountUpTimer());
        }
    }

    public void StopUsabilityTimer(UsabilityTimer game)
    {
        if (_currentGame != game)
        {
            return;
        }

        StopCoroutine(CO_CountUpTimer());

        if (game == UsabilityTimer.Initiator)
        {
            PlayerPrefs.SetInt("UsabilityInitiator", _timer);
            return;
        }

        _durations[(int)game] = _timer;
    }

    public void ShowDurations()
    {
        int seconds;
        int minutes;

        for (int i = 0; i < _texts.Count; i++)
        {
            minutes = Mathf.FloorToInt(_durations[i] / 60);
            seconds = Mathf.FloorToInt(_durations[i] - (minutes * 60));

            _texts[i].text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }

        _durationGO.SetActive(true);
    }

    private IEnumerator CO_CountUpTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            _timer++;
        }
    }
}
