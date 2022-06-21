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

    private UnityAction _transitionAction;
    private UnityAction _onComplete;

    private bool _rotateAfter = false;

    private bool _isInsideElevator = false;
    private bool _isLookingBehind = false;

    [HideInInspector] public bool _isGoingToGame3 = false;

    #endregion

    #region Rotation
    public void Rotate()
    {
        StopCoroutine(Transition(true));
        StartCoroutine(Transition(true));

        _playerRotatePnl.SetActive(false);
    }

    public void ShowRotateButton()
    {
        _playerRotatePnl.SetActive(true);
    }
    public void CopyRotation()
    {
        if (_isLookingBehind)
            _playerLocation.localEulerAngles = Vector3.zero;
        else
            _playerLocation.localEulerAngles = new Vector3(0, 180, 0);
    }
    #endregion

    #region Teleportation
    public void Teleport()
    {
        StopCoroutine(Transition(false));
        StartCoroutine(Transition(false));

        _playerTeleportPnl.SetActive(false);
    }

    public void ShowTeleportButton()
    {
        _playerTeleportPnl.SetActive(true);

        if (!_isInsideElevator)
            _rotateAfter = true;
    }

    private void CopyPosition()
    {
        if (!_isInsideElevator)
            _playerLocation.localPosition = Vector3.zero;
        else
            _playerLocation.localPosition = new Vector3(0, 0, 1.5f);

        if (_isGoingToGame3)
        {
            _playerLocation.localPosition = new Vector3(0, 0, 2.5f);
            _isGoingToGame3 = false;
        }
    }
    #endregion

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

        _isInsideElevator = _playerLocation.localPosition.z < 1.5f;
        _isLookingBehind = _playerLocation.localEulerAngles.y == 180;
    }
}
