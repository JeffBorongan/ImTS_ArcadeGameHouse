using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.InputSystem.InputAction;

public class CharacterControls : MonoBehaviour
{
    public void Rotate(CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
    }
}
