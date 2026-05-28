using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerPaddleController : MonoBehaviour
{
    public float movespeed = 20f;

    [Header("Bounds")]
    public float minX = -4.5f;
    public float maxX = 4.5f;
    public float minZ = -9f;
    public float maxZ = 0f;

    [Header("Mouse Settings")]
    public LayerMask tablelayer;

    private Rigidbody rb;
    private Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        Cursor.lockState = CursorLockMode.Confined;
    }

    void FixedUpdate()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, tablelayer))
        {
            Vector3 targetPosition = hit.point;

            // Clamp inside player area
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);

            // Keep paddle height
            targetPosition.y = rb.position.y;

            // Smooth movement
            Vector3 newPosition = Vector3.MoveTowards(
                rb.position,
                targetPosition,
                movespeed * Time.fixedDeltaTime
            );

            rb.MovePosition(newPosition);
        }
    }
}