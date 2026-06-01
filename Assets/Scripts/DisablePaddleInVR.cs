using UnityEngine;


public class DisablePaddleInVR : MonoBehaviour
{
    private MonoBehaviour paddleController;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Start()
    {
        paddleController = GetComponent<PaddleController>();
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        grab.selectEntered.AddListener(_ => paddleController.enabled = false);
        grab.selectExited.AddListener(_ => paddleController.enabled = true);
    }
}