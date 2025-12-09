using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    //[SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject resumeButton,returnButton, exitButton;


    private bool isPaused = false;

    // Optional: assign the gamepad button to pause (e.g., Start button)
    public Gamepad gamepad;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        // Keyboard input
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }

        // Gamepad input
        gamepad = Gamepad.current; // Get the first connected gamepad
        if (gamepad != null)
        {
            // Common pause buttons: Start, Menu, Options (check your gamepad type)
            if (gamepad.startButton.wasPressedThisFrame || gamepad.selectButton.wasPressedThisFrame)
            {
                TogglePause();
            }
        }
    }

    private void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        //gameplayUI.SetActive(false);
        PauseManager.Pause();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        //gameplayUI.SetActive(true);
        PauseManager.Resume();
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Customize()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}

