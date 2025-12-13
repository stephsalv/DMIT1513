using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pauseClip;

    private bool isPaused = false;


    void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.instance.PauseMenuInput)
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }
    public void ReturnToTitle()
    {
        Time.timeScale = 1f; // Reset time before switching scenes
        SceneManager.LoadScene(0);
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
        isPaused = true;

        if (audioSource && pauseClip)
            audioSource.PlayOneShot(pauseClip);
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;
    }
}
