using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets.DynamicMoveProvider;

[RequireComponent(typeof(Rigidbody))]
public class AirHockeyAgent : Agent
{
    [Header("Referenties")]
    public GameManager gameManager;
    public Transform puckTransform;
    public Rigidbody puckRigidbody;

    [Header("Instellingen")]
    public float speed = 15f;

    [Header("Grenzen")]
    public float minX = -4.25f;
    public float maxX = 4.25f;
    public float minZ = 0f;
    public float maxZ = 9f;

    private Vector3 movementDirection;

    private Rigidbody rb;
    private Vector3 targetPosition;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = rb.position;
    }

    public override void OnEpisodeBegin()
    {
        if (gameManager != null && Academy.Instance.IsCommunicatorOn)
        {
            gameManager.ResetAfterGoal();
        }
        targetPosition = rb.position;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(puckTransform.position);
        sensor.AddObservation(puckRigidbody.linearVelocity);

        Vector3 toPuck = puckTransform.position - transform.position;
        sensor.AddObservation(new Vector2(toPuck.x, toPuck.z));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        movementDirection = new Vector3(moveX, 0f, moveZ);

        AddReward(-0.001f);
    }

    void FixedUpdate()
    {
        if (rb.isKinematic)
        {
            Vector3 nextPos = rb.position + movementDirection * speed * Time.fixedDeltaTime;

            nextPos.x = Mathf.Clamp(nextPos.x, minX, maxX);
            nextPos.z = Mathf.Clamp(nextPos.z, minZ, maxZ);

            rb.MovePosition(nextPos);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        float x = 0f;
        float z = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) z = 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) z = -1f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) x = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x = 1f;

        continuousActions[0] = x;
        continuousActions[1] = z;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Puck"))
        {
            AddReward(0.1f);
        }
    }
}