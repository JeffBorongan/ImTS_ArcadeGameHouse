using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;

public class FloatingUIManager : MonoBehaviour
{
    public static FloatingUIManager Instance { private set; get; }

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

    [SerializeField] private TextMeshProUGUI txtCountDownTimer = null;
    [SerializeField] private TextMeshProUGUI txtTimer = null;
    [SerializeField] private TextMeshProUGUI txtGameResult = null;
    [SerializeField] private TextMeshProUGUI txtPointEarned = null;

    private bool timerStarted = false;
    private IEnumerator timerEnumerator = null;

    private void Start()
    {
        GameExecution.Instance.OnEventRegistration.AddListener(() =>
        {
            BowlingGameManagement.Instance.OnGameStart.AddListener(HandleOnGameStart);

            GameManager.Instance.OnPointsUpdate.AddListener(HandleOnUpdatePoints);
            GameManager.Instance.OnGameEnd.AddListener(HandleOnGameEnd);
            GameManager.Instance.OnGameUpdate.AddListener(HandleOnGameUpdate);
            GameManager.Instance.OnCountDownStart.AddListener(HandleOnCountdownStart);
        });
    }

    private void HandleOnUpdatePoints(int point)
    {
        SetPointsEarned(point);
    }

    private void HandleOnCountdownStart(int duration, UnityAction callback)
    {
        SetDefault();
        StartCountdown(5, callback);
    }

    private void HandleOnGameUpdate(Level level, UnityAction callback)
    {
        //StopTimer();
        //StartTimer(TimeSpan.FromSeconds(level.timeToBeat), callback);
    }

    private void HandleOnGameStart(SessionData data)
    {
        //StartTimer(TimeSpan.FromSeconds(data), callback);
    }

    private void HandleOnGameEnd(bool win)
    {
        ShowGameResult(win);
        StopTimer();
    }

    public void StartCountdown(int countdownDuration, UnityAction OnCompleteCallback)
    {
        StartCoroutine(CountdownCour(countdownDuration, OnCompleteCallback));
    }

    IEnumerator CountdownCour(int duration, UnityAction OnCompleteCallback)
    {
        txtCountDownTimer.gameObject.SetActive(true);
        txtCountDownTimer.transform.localScale = Vector3.one;

        int currentCountDown = duration;

        while (currentCountDown >= 0)
        {
            txtCountDownTimer.text = currentCountDown.ToString();
            txtCountDownTimer.transform.DOScale(2, 0.1f).OnComplete(() =>
            {
                txtCountDownTimer.transform.DOScale(1, 0.1f);
            });
            yield return new WaitForSeconds(1);
            currentCountDown--;
        }

        txtCountDownTimer.transform.DOScale(0, 0.2f).OnComplete(() => { txtCountDownTimer.gameObject.SetActive(false); });
        OnCompleteCallback.Invoke();
        txtPointEarned.gameObject.SetActive(true);
    }


    public void StartTimer(TimeSpan timeSpan, UnityAction OnCompleteCallback)
    {
        timerEnumerator = TimerCour(timeSpan, OnCompleteCallback);
        StartCoroutine(timerEnumerator);
    }

    public void StopTimer()
    {
        StopCoroutine(timerEnumerator);
        timerStarted = false;
    }

    IEnumerator TimerCour(TimeSpan timeSpan, UnityAction OnCompleteCallback)
    {
        txtTimer.gameObject.SetActive(true);
        timerStarted = true;
        TimeSpan currentTimeSpan = timeSpan;

        while (currentTimeSpan.Seconds >= 0 && timerStarted)
        {
            txtTimer.text = string.Format("{0:D2}:{1:D2}", currentTimeSpan.Minutes, currentTimeSpan.Seconds);
            yield return new WaitForSeconds(1);
            currentTimeSpan -= TimeSpan.FromSeconds(1);
        }

        OnCompleteCallback.Invoke();
        txtPointEarned.gameObject.SetActive(false);
    }

    public void ShowGameResult(bool win)
    {
        txtGameResult.gameObject.SetActive(true);
        txtGameResult.text = win ? "Success" : "Failed";
        txtGameResult.color = win ? Color.green : Color.red;
        txtGameResult.transform.localScale = Vector3.zero;
        txtGameResult.transform.DOScale(1, 0.2f);
    }

    public void SetPointsEarned(int points)
    {
        txtPointEarned.text = points.ToString();
    }

    public void SetDefault()
    {
        StopAllCoroutines();
        txtTimer.gameObject.SetActive(false);
        txtCountDownTimer.gameObject.SetActive(false);
        txtGameResult.gameObject.SetActive(false);
        txtPointEarned.gameObject.SetActive(false);
    }

}
