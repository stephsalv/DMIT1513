using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameMenu;

    [Header("Button to highlight")]
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject pauseMenuButton;

    [Header("References")]
    [SerializeField] public List<PlayerController> players;
    [SerializeField] public List<Ghost> ghosts;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pauseClip;
    [SerializeField] private AudioSource bgMusic;

    private bool isPaused = false;

    private void Start()
    {
        pauseMenu.SetActive(false);
        gameMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(mainMenuButton);
    }

    private void Update()
    {
        if ((Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame) ||
            (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame))
        {
            if (!isPaused)
                Pause();
            else
                Unpause();
        }

        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            ShowGameOver();
        }
    }

    public void OnPauseUIButtonPressed()
    {
        if (!isPaused)
            Pause();
    }

    public void OnResumeUIButtonPressed()
    {
        Unpause();
    }
    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        pauseMenu.SetActive(true);
        gameMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(pauseMenuButton);//in-game scene has quit button


        foreach (var p in players)
            if (p != null) p.enabled = false;

        foreach (var g in ghosts)
            if (g != null) g.enabled = false;

        if (audioSource && pauseClip)
            audioSource.PlayOneShot(pauseClip);

        if (bgMusic && bgMusic.isPlaying)
            bgMusic.Pause();
    }
    private void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        pauseMenu.SetActive(false);
        gameMenu.SetActive(true);

        StartCoroutine(SelectNextFrame(mainMenuButton));

        foreach (var p in players)
            if (p != null) p.enabled = true;

        foreach (var g in ghosts)
            if (g != null) g.enabled = true;

        if (bgMusic)
            bgMusic.UnPause();
    }
    private void ShowGameOver()
    {
        Time.timeScale = 0f;

        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);

        if (bgMusic && bgMusic.isPlaying)
            bgMusic.Stop();
    }
    private IEnumerator SelectNextFrame(GameObject button)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(button);
    }
    public void OnMainMenuButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void OnQuitUIButtonPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
