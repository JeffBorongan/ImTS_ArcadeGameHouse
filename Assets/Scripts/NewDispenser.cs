namespace UnityEngine.XR.Interaction.Toolkit
{
    /// <summary>
    /// This is the simplest version of an Interactable object.
    /// It simply provides a concrete implementation of the <see cref="XRBaseInteractable"/>.
    /// It is intended to be used as a way to respond to <see cref="XRBaseInteractable.onHoverEntered"/>/<see cref="XRBaseInteractable.onHoverExited"/>
    /// and <see cref="XRBaseInteractable.onSelectEntered"/>/<see cref="XRBaseInteractable.onSelectExited"/>/<see cref="XRBaseInteractable.onSelectCanceled"/>
    /// events with no underlying interaction behavior.
    /// </summary>
    [SelectionBase]
    [DisallowMultipleComponent]
    [AddComponentMenu("XR/Dispenser")]
    [HelpURL(XRHelpURLConstants.k_XRSimpleInteractable)]
    public class NewDispenser : XRBaseInteractable
    {
        [SerializeField] private GameObject ObjectToDispense = null;
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            Debug.Log("Dispense");

            GameObject clone = Instantiate(ObjectToDispense, transform.position, transform.rotation);

            XRGrabInteractable interactable = clone.GetComponent<XRGrabInteractable>();
            interactable.selectEntered.Invoke(args);
        }
    }
}
