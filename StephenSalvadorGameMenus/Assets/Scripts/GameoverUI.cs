using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Vehicles.Car;

public class GameOverUI : MonoBehaviour
{
    public SaveSystem saveSystem;
    public CarAudio[] playerCarAudio;

    public GameObject gameWonPanel;
    public GameObject gameLostPanel;

    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI resultsText;

    private SaveProfile currentProfile;
    private float lastRaceTime;
    private GhostData lastGhostData;

    public void ShowGameWon(SaveProfile profile, float raceTime, GhostData ghostData)
    {
        currentProfile = profile;
        lastRaceTime = raceTime;
        lastGhostData = ghostData;

        Time.timeScale = 0f;
        MuteAllCarAudio();

        resultsText.text =
            $"Profile: {profile.profileName}\n" +
            $"Previous Best: {profile.bestTime:F2}\n" +
            $"New Time: {raceTime:F2}";

        feedbackText.text = "Would you like to save and overwrite your profile?";
        gameWonPanel.SetActive(true);
        gameLostPanel.SetActive(false);
    }

    public void ShowGameLost(SaveProfile profile, float raceTime, GhostData ghostData)
    {
        currentProfile = profile;
        lastRaceTime = raceTime;
        lastGhostData = ghostData;

        Time.timeScale = 0f;
        MuteAllCarAudio();

        resultsText.text =
            $"Profile: {profile.profileName}\n" +
            $"Best Time: {profile.bestTime:F2}\n" +
            $"Race Time: {raceTime:F2}";

        feedbackText.text = "You lost! Try again?";
        gameLostPanel.SetActive(true);
        gameWonPanel.SetActive(false);
    }

    private void MuteAllCarAudio()
    {
        foreach (var carAudio in playerCarAudio)
        {
            if (carAudio != null)
                carAudio.MuteAudio();
        }
    }

    private void UnmuteAllCarAudio()
    {
        foreach (var carAudio in playerCarAudio)
        {
            if (carAudio != null)
                carAudio.UnmuteAudio();
        }
    }

    public void SaveAndOverwrite()
    {
        if (currentProfile == null) return;

        if (currentProfile.bestTime == 0f || lastRaceTime < currentProfile.bestTime)
        {
            currentProfile.bestTime = lastRaceTime;
            currentProfile.ghostData = lastGhostData;
        }

        saveSystem.CreateSaveData(currentProfile);
        feedbackText.text = "Saved successfully!";
        StartCoroutine(AutoReturnToMenu(2f));
    }

    public void DontSave()
    {
        feedbackText.text = "Progress not saved.";
        StartCoroutine(AutoReturnToMenu(2f));
    }

    private IEnumerator AutoReturnToMenu(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        UnmuteAllCarAudio();
        SceneManager.LoadScene(0);
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
