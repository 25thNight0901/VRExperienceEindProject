using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform puck;
    public Transform puckSpawn;

    public int playerScore = 0;
    public int aiScore = 0;

    public void ScoreGoal(string goalName)
    {
        if (goalName == "GoalLeft")
        {
            aiScore++;
            Debug.Log("AI Scores! " + aiScore);
        }
        else if (goalName == "GoalRight")
        {
            playerScore++;
            Debug.Log("Player Scores! " + playerScore);
        }

        ResetPuck();
    }

    void ResetPuck()
    {
        puck.position = puckSpawn.position;

        Rigidbody rb = puck.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}