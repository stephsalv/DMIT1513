using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu, pauseButton, playButton, exitButton;
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
        pauseButton.SetActive(false);
        playButton.SetActive(false);
        exitButton.SetActive(false);
        isPaused = true;
    }
    public void Resume()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        playButton.SetActive(true);
        exitButton.SetActive(true);
        isPaused = false;
    }
}
