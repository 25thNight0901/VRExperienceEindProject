using UnityEngine;
using Unity.MLAgents;

public class GameManager : MonoBehaviour
{
    [Header("Puck")]
    public Transform puck;

    [Header("Paddles")]
    public Rigidbody playerPaddle;
    public Rigidbody aiPaddle;

    [Header("Spawn Points")]
    public Transform startSpawn;

    [Header("Paddle Spawn Points")]
    public Transform playerPaddleSpawn;
    public Transform aiPaddleSpawn;

    [Header("Score")]
    public int playerScore = 0;
    public int aiScore = 0;

    [Header("AIAgents")]
    public AirHockeyAgent aiAgent;

    [Header("Training")]
    public float maxEpisodeDuration = 15f;
    public float puckIdleTimeout = 3f;
    public float puckIdleThreshold = 0.1f;

    private float episodeTimer = 0f;
    private float puckIdleTimer = 0f;
    private bool puckHasBeenHit = false;

    private Rigidbody puckRb;

    void Start()
    {
        puckRb = puck.GetComponent<Rigidbody>();

        SpawnAtStart();
    }

    void Update()
    {
        if (!Academy.Instance.IsCommunicatorOn)
        {
            return;
        }

        episodeTimer += Time.deltaTime;

        if (puck.position.z > aiAgent.maxZ/2)
        {
            aiAgent.AddReward(-0.2f);
            ResetTimers();
            aiAgent.EndEpisode();
            return;
        }

        if (puckHasBeenHit && puckRb.linearVelocity.magnitude < puckIdleThreshold)
        {
            puckIdleTimer += Time.deltaTime;
        }
        else
        {
            puckIdleTimer = 0f;
        }
        if (episodeTimer>= maxEpisodeDuration || puckIdleTimer >= puckIdleTimeout)
        {
            aiAgent.AddReward(-0.1f);// penalty for stalling
            ResetTimers();
            aiAgent.EndEpisode();
        }
    }

    void ResetRigidbody(Rigidbody rb, Transform spawn)
    {
        if (!rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }

        rb.position = spawn.position;
        rb.rotation = spawn.rotation;
    }

    void ResetPuckRandom()
    {
        float randomX = Random.Range(aiAgent.minX, aiAgent.maxX);
        float randomZ = Random.Range(aiAgent.minZ, aiAgent.maxZ/2);

        Vector3 randomPos = new Vector3(randomX, puck.position.y, randomZ);

        puckRb.linearVelocity = Vector3.zero;
        puckRb.angularVelocity = Vector3.zero;
        puckRb.Sleep();
        puckRb.position = randomPos;
    }

    void ResetTimers()
    {
        episodeTimer = 0f;
        puckIdleTimer = 0f;
        puckHasBeenHit = false;
    }

    public void NotifyPuckHit()
    {
        puckHasBeenHit = true;
    }


    void SpawnAtStart()
    {
        ResetPuckRandom();
        //ResetRigidbody(playerPaddle, playerPaddleSpawn);
        ResetRigidbody(aiPaddle, aiPaddleSpawn);
    }

    public void ResetAfterGoal()
    {
        ResetTimers();
        //ResetRigidbody(playerPaddle, playerPaddleSpawn);
        ResetRigidbody(aiPaddle, aiPaddleSpawn);
        ResetPuckRandom();
    }

    public void ScoreGoal(string goalName)
    {
        ResetTimers();
        if (goalName == "PlayerGoal")
        {
            aiScore++;
            Debug.Log("AI Scores! " + aiScore);

            aiAgent.AddReward(1.0f);

            aiAgent.EndEpisode();
        }
        else if (goalName == "AIGoal")
        {
            playerScore++;
            Debug.Log("Player Scores! " + playerScore);

            aiAgent.AddReward(-1.0f);

            aiAgent.EndEpisode();
        }
    }
}