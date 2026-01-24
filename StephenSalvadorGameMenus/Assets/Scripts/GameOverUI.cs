using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("Settings")]
    public SaveSystem saveSystem; // assign your SaveSystem instance in inspector
    public string vehicleName;     // vehicle to save for the profile

    // Reloads the current scene
    public void ReplayRace()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void SaveProfile()
    {
        if (saveSystem != null)
        {
            saveSystem.SaveNewProfile(vehicleName);
        }
        else
        {
            Debug.LogWarning("SaveSystem not assigned!");
        }
    }
}
