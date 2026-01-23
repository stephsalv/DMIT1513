using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int index;
    public Checkpoint[] checkpoints;
    public FinalCheckpoint finalCheckpoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player hit checkpoint {index}");

            gameObject.SetActive(false);

            if (index == checkpoints.Length - 1)
            {
                finalCheckpoint.gameObject.SetActive(true);
            }
            else
            {
                checkpoints[index + 1].gameObject.SetActive(true);
            }
        }
    }
}

