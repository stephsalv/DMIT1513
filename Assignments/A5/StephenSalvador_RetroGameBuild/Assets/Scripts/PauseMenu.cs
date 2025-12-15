using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameMenuUI;
    [SerializeField] private GameObject resumeButton;

    [Header("Gameplay References")]
    [SerializeField] private List<PlayerController> players;
    [SerializeField] private List<Ghost> ghosts;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pauseClip;
    [SerializeField] private AudioSource bgMusic;

    private bool isPaused = false;
    private int pausingPlayerId = -1;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        gameMenuUI.SetActive(true);
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Debug.Log($"Pause input detected from device: {context.control.device.displayName} ({context.control.device.GetType().Name})");

        if (!(context.control.device is Gamepad || context.control.device is Joystick))
        {
            Debug.Log("Ignored: Not a controller");
            return;
        }

        int playerId = context.control.device.deviceId;

        if (!isPaused)
        {
            pausingPlayerId = playerId;
            Debug.Log($"Game paused by player {playerId}");
            Pause();
        }
        else
        {
            if (pausingPlayerId == playerId)
            {
                Debug.Log($"Game resumed by player {playerId}");
                Unpause();
                pausingPlayerId = -1;
            }
            else
            {
                Debug.Log($"Player {playerId} tried to resume but isn't the one who paused.");
            }
        }
    }
    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        pauseMenuUI.SetActive(true);
        gameMenuUI.SetActive(false);

        EventSystem.current.SetSelectedGameObject(resumeButton);

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

        pauseMenuUI.SetActive(false);
        gameMenuUI.SetActive(true);

        foreach (var p in players)
            if (p != null) p.enabled = true;

        foreach (var g in ghosts)
            if (g != null) g.enabled = true;

        if (bgMusic)
            bgMusic.UnPause();
    }
    public void OnResumePressed(int playerId = -1)
    {
        if (!isPaused)
            return;

        if (playerId == -1 || playerId == pausingPlayerId)
        {
            Unpause();
            pausingPlayerId = -1;
        }
    }
    public void OnExitPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
