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
        Vector3 pos = rb.position;
        pos.y = lockedY;
        rb.MovePosition(pos);

        Vector3 vel = rb.linearVelocity;
        vel.y = 0f;
        rb.linearVelocity = vel;

        Vector3 angVel = rb.angularVelocity;
        angVel.x = 0f;
        angVel.z = 0f;
        rb.angularVelocity = angVel;
    }
}