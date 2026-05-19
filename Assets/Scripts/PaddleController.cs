using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerPaddleController : MonoBehaviour
{
    public float speed = 15f;

    [Header("Bounds")]
    public float minX = -4.5f;
    public float maxX = 4.5f;
    public float minZ = -9f;
    public float maxZ = 0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ);

        Vector3 newPosition =
            rb.position + movement * speed * Time.fixedDeltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

        rb.MovePosition(newPosition);
    }
}