using Unity.VisualScripting;
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
            Debug.Log("PLAYER WINS!");
            // TODO: Show Win UI, save ghost, stop timer
        }
        else if (other.CompareTag("AICar"))
        {
            raceFinished = true;
            Debug.Log("AI WINS!");
            // TODO: Show Lose UI
        }
    }
}
