using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject gameOverPanel;

    private void Start()
    {
        // Make cursor visible and unlocked at start
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // Replay Level 0
    public void ReplayLevel1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // Replay Level 1
    public void ReplayLevel2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    // Replay Level 2
    public void ReplayLevel3()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }

    // Quit Game
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
