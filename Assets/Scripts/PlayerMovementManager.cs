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

    [SerializeField] private bool _initializer;

    [SerializeField] private Transform _playerLocation;

    [SerializeField] private GameObject _playerTeleportPnl = null;
    [SerializeField] private GameObject _playerRotatePnl = null;

    [Header("Audio")]
    [SerializeField] private AudioClip _teleportVoiceOver = null;
    [SerializeField] private AudioClip _rotateVoiceOver = null;

    private UnityAction _transitionAction;
    private UnityAction _onComplete;

    private bool _rotateAfter = false;
    private bool _teleportAfter = false;

    private bool _isInsideElevator = false;
    private bool _isLookingBehind = false;

    [HideInInspector] public bool _isGoingToGame3 = false;

    #endregion

    private void Start()
    {
        if (_initializer)
            _teleportAfter = true;
    }

    #region Rotation
    public void Rotate()
    {
        StopCoroutine(Transition(true));
        StartCoroutine(Transition(true));

        if(!_initializer)
            _playerRotatePnl.SetActive(false);
    }

    public void ShowRotateButton()
    {
        VoiceOverManager.Instance.PlayClip(_rotateVoiceOver);
        _playerRotatePnl.SetActive(true);
    }

    public void CopyRotation()
    {
        if (_isLookingBehind)
        {
            // looking behind, so I will look at front
            _playerLocation.localEulerAngles = Vector3.zero;
        }
        else
        {
            // looking at front, so I will look behind
            _playerLocation.localEulerAngles = new Vector3(0, 180, 0);
        }
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
        VoiceOverManager.Instance.PlayClip(_teleportVoiceOver);

        _playerTeleportPnl.SetActive(true);

        if (!_isInsideElevator && !_initializer)
            _rotateAfter = true;
    }

    private void CopyPosition()
    {
        if (!_isInsideElevator)
        {
            _playerLocation.localPosition = new Vector3(0, -0.5f, 0);
            Debug.Log("not inside elevator, going in");
        }
        else
        {
            _playerLocation.localPosition = new Vector3(0, -0.5f, 1.5f);
            Debug.Log("inside elevator, going out");
        }

        if (_isGoingToGame3)
        {
            _playerLocation.localPosition = new Vector3(0, 0, 2.5f);
            _isGoingToGame3 = false;
        }
    }

    public void TeleportAfter()
    {
        _teleportAfter = true;
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

        _onComplete -= ShowTeleportButton;
        if (_teleportAfter)
        {
            _onComplete += ShowTeleportButton;
            _teleportAfter = false;
        }

        ScreenFadeManager.Instance.FadeIn(_transitionAction);
        yield return new WaitForSeconds(0.5f);
        ScreenFadeManager.Instance.FadeOut(_onComplete);

        _isInsideElevator = _playerLocation.localPosition.z < 1.5f;
        _isLookingBehind = _playerLocation.localEulerAngles.y == 180;
    }
}
