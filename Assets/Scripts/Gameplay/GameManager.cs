using UnityEngine;
using Unity.MLAgents;

public class GameManager : MonoBehaviour
{
    [Header("Puck")]
    public Transform puck;

    [Header("Paddles")]
    public Rigidbody playerPaddle;
    public Rigidbody aiPaddle;

    [Header("Puck Spawn Points")]
    public Transform startSpawn;
    public Transform playerSpawn;
    public Transform aiSpawn;

    [Header("Paddle Spawn Points")]
    public Transform playerPaddleSpawn;
    public Transform aiPaddleSpawn;

    [Header("Score")]
    public int playerScore = 0;
    public int aiScore = 0;

    [Header("Game")]
    public int winningScore = 7;

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
        ResetRigidbody(puckRb, startSpawn);
        ResetRigidbody(playerPaddle, playerPaddleSpawn);
        ResetRigidbody(aiPaddle, aiPaddleSpawn);
    }

    public void ResetAfterGoal()
    {
        ResetTimers();
        ResetRigidbody(playerPaddle, playerPaddleSpawn);
        ResetRigidbody(aiPaddle, aiPaddleSpawn);
        ResetRigidbody(puckRb, startSpawn);
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

            ResetAfterGoal();

            puck.position = playerSpawn.position;
        }
        else if (goalName == "AIGoal")
        {
            playerScore++;
            Debug.Log("Player Scores! " + playerScore);

            aiAgent.AddReward(-1.0f);

            aiAgent.EndEpisode();

            ResetAfterGoal();

            puck.position = aiSpawn.position;
        }
        if (playerScore >= winningScore)
        {
            Debug.Log("PLAYER WINS!");
            EndMatch();
            return;
        }

        if (aiScore >= winningScore)
        {
            Debug.Log("AI WINS!");
            EndMatch();
            return;
        }
        void EndMatch()
        {
            Debug.Log("Match Over");

            puckRb.linearVelocity = Vector3.zero;
        }
    }
}