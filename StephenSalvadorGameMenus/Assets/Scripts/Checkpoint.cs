using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int index;
    public Checkpoint[] checkpoints;
    public FinalCheckpoint finalCheckpoint;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log($"Player hit checkpoint {index}");

            // Disable this checkpoint
            gameObject.SetActive(false);

            // If this is the last checkpoint, enable FinalCheckpoint
            if (index == checkpoints.Length - 1)
            {
                finalCheckpoint.gameObject.SetActive(true);
            }
            else
            {
                // Enable next checkpoint
                checkpoints[index + 1].gameObject.SetActive(true);
            }
        }
        else if (other.CompareTag("AICar"))
        {
            triggered = true;
            Debug.Log($"AICar hit checkpoint {index}");

            // Disable this checkpoint
            gameObject.SetActive(false);

            // If this is the last checkpoint, enable FinalCheckpoint
            if (index == checkpoints.Length - 1)
            {
                finalCheckpoint.gameObject.SetActive(true);
            }
            else
            {
                // Enable next checkpoint
                checkpoints[index + 1].gameObject.SetActive(true);
            }
        }
    }
}

