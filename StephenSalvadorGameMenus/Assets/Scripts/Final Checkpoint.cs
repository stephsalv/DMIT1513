using UnityEngine;

public class FinalCheckpoint : MonoBehaviour
{
    private bool raceFinished = false;

    public GameOverUI gameOverUI;
    public TimerScript timer;
    public SaveProfile currentProfile;
    public GhostData currentGhostData;

    private void OnTriggerEnter(Collider other)
    {
        if (raceFinished) return;

        if (other.CompareTag("Player"))
        {
            raceFinished = true;
            timer.StopTimer();
            gameOverUI.ShowGameWon(
                currentProfile,
                timer.GetFinalTime(),
                currentGhostData
            );
        }
        else if (other.CompareTag("AICar"))
        {
            raceFinished = true;
            timer.StopTimer();
            gameOverUI.ShowGameLost(
                currentProfile,
                timer.GetFinalTime(),
                currentGhostData
            );
        }
    }
}
