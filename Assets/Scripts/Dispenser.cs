using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Dispenser : XRBaseInteractable
{
    [SerializeField] private GameObject objectToDispense = null;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        GameObject clone = Instantiate(objectToDispense, transform.position, transform.rotation);
        XRGrabInteractable interactable = clone.GetComponent<XRGrabInteractable>();

        interactable.selectEntered.Invoke(args);
    }
}
