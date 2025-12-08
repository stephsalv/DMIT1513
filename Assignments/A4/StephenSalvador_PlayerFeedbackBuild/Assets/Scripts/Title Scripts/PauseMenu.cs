using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu, gameplayUI;
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    private bool isPaused = false;

    void Start()
    {
        pauseMenu .SetActive(false);

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
        }
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
    public void Play()
    {
        Time.timeScale = 1f; // Reset time before switching scenes
        SceneManager.LoadScene(1);
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
        gameplayUI.SetActive(false); // Hide everything else
        isPaused = true;

        // Show and unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        gameplayUI.SetActive(true); // Show gameplay UI again
        isPaused = false;

        // Hide and lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ResumeTitlePage()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        gameplayUI.SetActive(true); // Show gameplay UI again
        isPaused = false;
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
    }
}
