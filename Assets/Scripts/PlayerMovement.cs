using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _playerLocation;

    [SerializeField] private GameObject _playerTeleportPnl;
    [SerializeField] private GameObject _playerRotatePnl;
    [SerializeField] private List<Button> _therapistButtons;

    private UnityAction action;

    public void Rotate()
    {
        StopCoroutine(TeleportWithFade(true));
        StartCoroutine(TeleportWithFade(true));

        _playerRotatePnl.SetActive(false);
        SetButtonInteractables(true);
    }

    public void Teleport()
    {
        StopCoroutine(TeleportWithFade(false));
        StartCoroutine(TeleportWithFade(false));

        _playerTeleportPnl.SetActive(false);
        SetButtonInteractables(true);
    }

    public void ShowTeleportButton()
    {
        _playerTeleportPnl.SetActive(true);
        SetButtonInteractables(false);
    }

    public void ShowRotateButton()
    {
        _playerRotatePnl.SetActive(true);
        SetButtonInteractables(false);
    }

    private void SetButtonInteractables(bool value)
    {
        foreach (Button button in _therapistButtons)
            button.interactable = value;
    }

    private void CopyPosition()
    {
        if (_playerLocation.localPosition.z == 1.5f)
            _playerLocation.localPosition = new Vector3(0, 0, 0);
        else
            _playerLocation.localPosition = new Vector3(0, 0, 1.5f);
    }

    public void CopyRotation()
    {
        if (_playerLocation.localEulerAngles.y == 180)
            _playerLocation.localEulerAngles = new Vector3(0, 0, 0);
        else
            _playerLocation.localEulerAngles = new Vector3(0, 180, 0);
    }

    private IEnumerator TeleportWithFade(bool isRotation)
    {
        action -= CopyRotation;
        action -= CopyPosition;
        if (isRotation)
            action += CopyRotation;
        else
            action += CopyPosition;

        ScreenFadeManager.Instance.FadeIn(action);
        yield return new WaitForSeconds(0.5f);
        ScreenFadeManager.Instance.FadeOut(() => EmptyFunction());
    }

    private void EmptyFunction()
    { }
}
