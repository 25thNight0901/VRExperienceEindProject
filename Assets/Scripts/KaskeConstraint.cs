using UnityEngine;

public class MalletConstraint : MonoBehaviour
{
    private float lockedY;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lockedY = transform.position.y;
    }

    void FixedUpdate()
    {
        // Lock Y position
        Vector3 pos = rb.position;
        pos.y = lockedY;
        rb.MovePosition(pos);

        // Kill vertical velocity only, preserve horizontal for puck hits
        Vector3 vel = rb.linearVelocity;
        vel.y = 0f;
        rb.linearVelocity = vel;

        // Kill vertical angular velocity only
        Vector3 angVel = rb.angularVelocity;
        angVel.x = 0f;
        angVel.z = 0f;
        rb.angularVelocity = angVel;
    }
}