using UnityEngine;


public class DisablePaddleInVR : MonoBehaviour
{
    private PlayerPaddleController paddleController;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Start()
    {
        paddleController = GetComponent<PlayerPaddleController>();
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (paddleController == null)
            Debug.LogWarning($"DisablePaddleInVR: PlayerPaddleController not found on '{name}'");
        if (grab == null)
            Debug.LogWarning($"DisablePaddleInVR: XRGrabInteractable not found on '{name}'");

        if (grab != null)
        {
            grab.selectEntered.AddListener(args =>
            {
                Debug.Log($"DisablePaddleInVR: selectEntered on '{name}'");
                if (paddleController != null) paddleController.enabled = false;
            });

            grab.selectExited.AddListener(args =>
            {
                Debug.Log($"DisablePaddleInVR: selectExited on '{name}'");
                if (paddleController != null) paddleController.enabled = true;
            });
        }
    }
}