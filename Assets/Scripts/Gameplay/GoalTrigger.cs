using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public string puckTag = "Puck";
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(puckTag))
        {
            gameManager.ScoreGoal(gameObject.name);
        }
    }
}
