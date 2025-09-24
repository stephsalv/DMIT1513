using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu, gameplayUI, pauseButton, playButton, exitButton;
    private bool isPaused = false;


    void Start()
    {
        pauseMenu .SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f; // Reset time before switching scenes
        SceneManager.LoadScene(0);
    }
    public void Customize()
    {
        Time.timeScale = 1f; // Reset time before switching scenes
        SceneManager.LoadScene(1);
    }
    public void Play()
    {
        Time.timeScale = 1f; // Reset time before switching scenes
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        gameplayUI.SetActive(false); // Hide everything else
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        gameplayUI.SetActive(true); // Show gameplay UI again
        isPaused = false;
    }

}
