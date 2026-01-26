using UnityEngine;

public class FinalCheckpoint : MonoBehaviour
{
    private bool raceFinished = false;

    public GameOverUI gameOverUI;
    public TimerScript timer;
    public GhostDataRecorder ghostRecorder;

    private SaveProfile currentProfile;

    private void Start()
    {
        currentProfile = GameManager.instance.currentProfile;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (raceFinished) return;

        if (other.CompareTag("Player"))
        {
            raceFinished = true;
            Debug.Log("Player passed the final checkpoint");

            timer.StopTimer();
            ghostRecorder.StopRecording();

            gameOverUI.ShowGameWon(
                currentProfile,
                timer.GetFinalTime(),
                ghostRecorder.GetGhostData()
            );
        }
        if (other.CompareTag("AICar"))
        {
            Debug.Log("AICar passed the final checkpoint");
            raceFinished = true;

            timer.StopTimer();
            ghostRecorder.StopRecording();

            gameOverUI.ShowGameLost(
                currentProfile,
                timer.GetFinalTime(),
                ghostRecorder.GetGhostData()
            );
        }
    }
}
