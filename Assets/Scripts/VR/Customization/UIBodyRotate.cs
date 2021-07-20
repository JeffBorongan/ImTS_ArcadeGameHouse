using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBodyRotate : MonoBehaviour
{
    [SerializeField] private Transform characterMimic = null;
    [SerializeField] Button btnRotateLeft = null;
    [SerializeField] Button btnRotateRight = null;
    [SerializeField] private float rotateSensitivity = 1f;

    private void Start()
    {
        btnRotateLeft.onClick.AddListener(() => HandleOnRotate(false));
        btnRotateRight.onClick.AddListener(() => HandleOnRotate(true));
    }

    private void HandleOnRotate(bool right)
    {
        characterMimic.eulerAngles += new Vector3(0f, right ? -rotateSensitivity : rotateSensitivity, 0f);
    }
}
