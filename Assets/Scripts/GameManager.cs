using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Puck")]
    public Transform puck;

    [Header("Paddles")]
    public Rigidbody playerPaddle;
    public Rigidbody aiPaddle;

    [Header("Spawn Points")]
    public Transform startSpawn;
    public Transform playerSpawn;
    public Transform aiSpawn;

    [Header("Paddle Spawn Points")]
    public Transform playerPaddleSpawn;
    public Transform aiPaddleSpawn;

    [Header("Score")]
    public int playerScore = 0;
    public int aiScore = 0;

    private Rigidbody puckRb;

    void Start()
    {
        puckRb = puck.GetComponent<Rigidbody>();

        SpawnAtStart();
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


    void SpawnAtStart()
    {
        ResetRigidbody(puckRb, startSpawn);
        ResetRigidbody(playerPaddle, playerPaddleSpawn);
        ResetRigidbody(aiPaddle, aiPaddleSpawn);
    }

    public void ResetAfterGoal()
    {
        ResetRigidbody(playerPaddle, playerPaddleSpawn);
        ResetRigidbody(aiPaddle, aiPaddleSpawn);
        ResetRigidbody(puckRb, startSpawn);
    }

    public void ScoreGoal(string goalName)
    {
        if (goalName == "GoalLeft")
        {
            aiScore++;
            Debug.Log("AI Scores! " + aiScore);

            ResetAfterGoal();

            puck.position = aiSpawn.position;
        }
        else if (goalName == "GoalRight")
        {
            playerScore++;
            Debug.Log("Player Scores! " + playerScore);

            ResetAfterGoal();

            puck.position = playerSpawn.position;
        }
    }
}