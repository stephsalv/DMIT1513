using UnityEngine;

public class FinalCheckpoint : MonoBehaviour
{
    private bool raceFinished;

    public GameOverUI gameOverUI;
    public TimerScript timer;

    private SaveProfile currentProfile;

    private void Start()
    {
        currentProfile = GameManager.instance.currentProfile;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (raceFinished) return;

        GhostDataRecorder recorder = other.GetComponent<GhostDataRecorder>();
        if (recorder == null) return;

        raceFinished = true;
        timer.StopTimer();
        recorder.StopRecording();

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player passed final checkpoint");

            gameOverUI.ShowGameWon(
                currentProfile,
                timer.GetFinalTime(),
                recorder.GetGhostData()
            );
        }
        else if (other.CompareTag("AICar"))
        {
            Debug.Log("AI Car passed final checkpoint");

            gameOverUI.ShowGameLost(
                currentProfile,
                timer.GetFinalTime(),
                recorder.GetGhostData()
            );
        }
    }
}
