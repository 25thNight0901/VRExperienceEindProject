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
    public Transform opponentGoal;
    public Transform ownGoal;
    public Rigidbody puckRigidbody;

    [Header("Instellingen")]
    public float speed = 15f;

    [Header("Grenzen")]
    public float minX = -4.25f;
    public float maxX = 4.25f;
    public float minZ = 0f;
    public float maxZ = 9f;

    private Rigidbody rb;
    private Vector3 targetPosition;
    private float prevDistToPuck;

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
        prevDistToPuck = Vector3.Distance(transform.position, puckTransform.position);
         targetPosition = rb.position;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Your own state
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rb.linearVelocity);

        Vector3 toPuck = puckTransform.position - transform.position;

        sensor.AddObservation(toPuck.normalized); // sensor for puck direction
        sensor.AddObservation(toPuck.magnitude); // sensor for distance to puck
        sensor.AddObservation(puckRigidbody.linearVelocity); //sensor for puck velocity

        //sensor for attack direction
        Vector3 toOpponentGoal = opponentGoal.position - puckTransform.position;
        sensor.AddObservation(toOpponentGoal.normalized);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 movementDirection = new Vector3(moveX, 0f, moveZ);

        Vector3 nextPos = rb.position + movementDirection * speed * Time.fixedDeltaTime;

        nextPos.x = Mathf.Clamp(nextPos.x, minX, maxX);
        nextPos.z = Mathf.Clamp(nextPos.z, minZ, maxZ);

        rb.MovePosition(nextPos);

        if (puckRigidbody.linearVelocity.magnitude> 0.5f)
        {
            float distToPuck = Vector3.Distance(transform.position, puckTransform.position);
            float delta = prevDistToPuck - distToPuck;
            AddReward(delta * 0.01f);
        }
        
        prevDistToPuck = Vector3.Distance(transform.position, puckTransform.position);
        AddReward(-0.0005f); // penalty for time wasting
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
            gameManager.NotifyPuckHit();
            AddReward(1f);
            EndEpisode();
        }
    }
}