using UnityEngine;


public class DisablePaddleInVR : MonoBehaviour
{
    private PlayerPaddleController paddleController;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Start()
    {
        paddleController = GetComponent<PlayerPaddleController>();
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        grab.selectEntered.AddListener(_ => paddleController.enabled = false);
        grab.selectExited.AddListener(_ => paddleController.enabled = true);
    }
}