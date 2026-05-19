using UnityEngine;

public class PlayerPaddleController : MonoBehaviour
{
    public float speed = 10f;

    [Header("Table Bounds")]
    public float minX = -4.5f;
    public float maxX = 4.5f;
    public float minZ = -9f;
    public float maxZ = 0f; // player side only

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ);

        transform.position += movement * speed * Time.deltaTime;

        // Clamp position
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.position = pos;
    }
}