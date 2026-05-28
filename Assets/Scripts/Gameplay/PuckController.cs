using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PuckController : MonoBehaviour
{
    public float maxSpeed = 15f;
    public float slowdown = 0.995f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Limit max speed
        rb.linearVelocity =
            Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);

        // Smooth slowdown
        rb.linearVelocity *= slowdown;
    }
}