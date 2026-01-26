using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public SaveSystem saveSystem;
    public GhostDataRecorder ghostRecorder;

    public GameObject gameWonPanel;
    public GameObject gameLostPanel;

    float lastRaceTime;
    GhostData lastGhostData;

    SaveProfile currentProfile;

    public void ShowGameWon(SaveProfile profile, float raceTime, GhostData ghostData)
    {
        currentProfile = profile;
        lastRaceTime = raceTime;
        lastGhostData = ghostData;

        Time.timeScale = 0f;
        gameWonPanel.SetActive(true);
    }

    public void ShowGameLost(SaveProfile profile, float raceTime, GhostData ghostData)
    {
        currentProfile = profile;
        lastRaceTime = raceTime;
        lastGhostData = ghostData;

        Time.timeScale = 0f;
        gameLostPanel.SetActive(true);
    }

    public void SaveProfile()
    {
        if (currentProfile == null)
        {
            Debug.LogWarning("No profile loaded");
            return;
        }

        float runTime = ghostRecorder.lapTime;

        if (currentProfile.bestTime == 0 || runTime < currentProfile.bestTime)
        {
            currentProfile.bestTime = runTime;
            currentProfile.ghostData = ghostRecorder.GetGhostData();
        }

        saveSystem.CreateSaveData(currentProfile);
        Debug.Log($"Saved profile: {currentProfile.profileName}");
    }

    public void ReplayRace()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
