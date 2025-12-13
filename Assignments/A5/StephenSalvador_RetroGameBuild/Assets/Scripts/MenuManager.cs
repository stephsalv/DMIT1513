using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject playButton;

    [Header("References")]
    [SerializeField] public List <PlayerController> players;
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
        EventSystem.current.SetSelectedGameObject(playButton);
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
                Unpause();
            }
        }
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        pauseMenu.SetActive(true);
        gameMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(resumeButton);

        // Disable players
        foreach (var p in players)
            if (p != null) p.enabled = false;

        // Disable ghosts
        foreach (var g in ghosts)
            if (g != null) g.enabled = false;

        if (audioSource && pauseClip)
            audioSource.PlayOneShot(pauseClip);

        if (bgMusic && bgMusic.isPlaying)
            bgMusic.Pause();
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        pauseMenu.SetActive(false);
        gameMenu.SetActive(true);

        // FIX: delayed selection
        StartCoroutine(SelectNextFrame(playButton));

        foreach (var p in players)
            if (p != null) p.enabled = true;

        foreach (var g in ghosts)
            if (g != null) g.enabled = true;

        if (bgMusic)
            bgMusic.UnPause();
    }
    public void OnResumePressed()
    {
        Unpause();
        StartCoroutine(SelectNextFrame(playButton));
    }
    private IEnumerator SelectNextFrame(GameObject button)
    {
        yield return null; // wait 1 frame
        EventSystem.current.SetSelectedGameObject(button);
    }
    public void OnExitPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
