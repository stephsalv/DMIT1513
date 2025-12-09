using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class MenuManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenu;

    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Ghost ghost;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pauseClip;

    [Header("Input")]
    [SerializeField] private InputAction pauseAction;

    [Header("First Selected Option")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject resumeButton;

    private bool isPaused = false;

    private void OnEnable()
    {
        pauseAction.Enable();
    }

    private void OnDisable()
    {
        pauseAction.Disable();
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            if (isPaused)
                Unpause();
            else
                Pause();
        }
    }

    private void Pause()
    {
        isPaused = true;

        Time.timeScale = 0f;

        if (player != null) player.enabled = false;
        if (ghost != null) ghost.enabled = false;

        OpenPauseMenu();

        if (audioSource != null && pauseClip != null)
            audioSource.PlayOneShot(pauseClip);
    }

    private void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    private void Unpause()
    {
        isPaused = false;

        Time.timeScale = 1f;

        if (player != null) player.enabled = true;
        if (ghost != null) ghost.enabled = true;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (audioSource != null && pauseClip != null)
            audioSource.PlayOneShot(pauseClip);
    }

    // Called by UI Button
    public void OnResumePressed()
    {
        Unpause();
    }

    // Called by UI Button
    public void OnExitPressed()
    {
        // Works in build
        Application.Quit();

        // Works in editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
