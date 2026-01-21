using UnityEngine;

public class FinalCheckpoint : MonoBehaviour
{
    private bool raceFinished = false;
    private void OnTriggerEnter(Collider other)
    {
        if (raceFinished) return;

        if (other.CompareTag("Player"))
        {
            raceFinished = true;
            Debug.Log("Player Wins!");
        }
        else if (other.CompareTag("AICar"))
        {
            raceFinished = true;
            Debug.Log("AI Wins!");
        }
    }
}
