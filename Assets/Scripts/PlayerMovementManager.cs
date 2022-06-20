using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerMovementManager : MonoBehaviour
{
    #region Singleton

    public static PlayerMovementManager Instance { private set; get; }

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

    [SerializeField] private Transform _playerLocation;

    [SerializeField] private GameObject _playerTeleportPnl = null;
    [SerializeField] private GameObject _playerRotatePnl = null;
    [SerializeField] private List<Button> _therapistButtons;

    private UnityAction _transitionAction;
    private UnityAction _onComplete;

    private bool _rotateAfter = false;

    #endregion

    #region Rotation
    public void Rotate()
    {
        StopCoroutine(Transition(true));
        StartCoroutine(Transition(true));

        _playerRotatePnl.SetActive(false);
        SetButtonInteractables(true);
    }

    public void ShowRotateButton()
    {
        _playerRotatePnl.SetActive(true);
        SetButtonInteractables(false);
    }
    public void CopyRotation()
    {
        if (_playerLocation.localEulerAngles.y == 180)
            _playerLocation.localEulerAngles = new Vector3(0, 0, 0);
        else
            _playerLocation.localEulerAngles = new Vector3(0, 180, 0);
    }

    public void SetRotateAfter(bool rotateAfter)
    {
        _rotateAfter = rotateAfter;
    }
    #endregion

    #region Teleportation
    public void Teleport()
    {
        StopCoroutine(Transition(false));
        StartCoroutine(Transition(false));

        _playerTeleportPnl.SetActive(false);
        SetButtonInteractables(true);
    }

    public void ShowTeleportButton()
    {
        _playerTeleportPnl.SetActive(true);
        SetButtonInteractables(false);
    }

    private void CopyPosition()
    {
        if (_playerLocation.localPosition.z == 1.5f)
            _playerLocation.localPosition = new Vector3(0, 0, 0);
        else
            _playerLocation.localPosition = new Vector3(0, 0, 1.5f);
    }
    #endregion

    private void SetButtonInteractables(bool value)
    {
        foreach (Button button in _therapistButtons)
            button.interactable = value;
    }

    private IEnumerator Transition(bool isRotation)
    {
        _transitionAction -= CopyRotation;
        _transitionAction -= CopyPosition;
        if (isRotation)
            _transitionAction += CopyRotation;
        else
            _transitionAction += CopyPosition;

        _onComplete -= ShowRotateButton;
        if (_rotateAfter)
        {
            _onComplete += ShowRotateButton;
            _rotateAfter = false;
        }

        ScreenFadeManager.Instance.FadeIn(_transitionAction);
        yield return new WaitForSeconds(0.5f);
        ScreenFadeManager.Instance.FadeOut(_onComplete);
    }
}
