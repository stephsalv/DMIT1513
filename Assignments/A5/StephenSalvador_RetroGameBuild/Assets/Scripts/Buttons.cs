using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject gameplayUI;
    public void On2PlayerPressed()
    {
        SceneManager.LoadScene(1);
    }
    public void On4PlayerPressed()
    {
        SceneManager.LoadScene(2);
    }

    // Called by the Quit button
    public void OnQuitPressed()
    {
        Application.Quit(); // Works in build

        // Works in Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    // ✔️ NEW — Called by the Resume button
    public void OnResumePressed()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (gameplayUI != null)
            gameplayUI.SetActive(true);

        // Unpause the game
        Time.timeScale = 1f;
    }
}

