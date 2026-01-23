using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Checkpoint[] checkpoints;
    public FinalCheckpoint finalCheckpoint;

    private void Awake()
    {
        foreach (Checkpoint cp in checkpoints)
        {
            cp.gameObject.SetActive(false);
        }

        finalCheckpoint.gameObject.SetActive(false);

        if (checkpoints.Length > 0)
        {
            checkpoints[0].gameObject.SetActive(true);
        }
    }
}
