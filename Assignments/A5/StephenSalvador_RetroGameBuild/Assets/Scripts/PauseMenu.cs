using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenu;
    public GameObject firstSelectedButton;

    [Header("Input Actions")]
    public InputActionReference pauseAction;   // Start button
    public InputActionReference cancelAction;  // B / Circle / Back button

    private bool isPaused = false;

    private void OnEnable()
    {
        pauseAction.action.Enable();
        cancelAction.action.Enable();

        pauseAction.action.performed += OnPausePressed;
        cancelAction.action.performed += OnCancelPressed;
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= OnPausePressed;
        cancelAction.action.performed -= OnCancelPressed;

        pauseAction.action.Disable();
        cancelAction.action.Disable();
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    // ---------------------------------------------------------
    // Start button toggles pause on/off
    // ---------------------------------------------------------
    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        if (isPaused)
            Unpause();
        else
            Pause();
    }

    // ---------------------------------------------------------
    // Cancel button exits menu (B / Circle / Back)
    // ---------------------------------------------------------
    private void OnCancelPressed(InputAction.CallbackContext ctx)
    {
        if (isPaused)
            Unpause();
    }

    // ---------------------------------------------------------
    // Pause Logic
    // ---------------------------------------------------------
    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        pauseMenu.SetActive(true);

        // Reset selection so controller likes it
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    // ---------------------------------------------------------
    // Unpause Logic
    // ---------------------------------------------------------
    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        pauseMenu.SetActive(false);

        // Clear selection when closing
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Button click calls this
    public void OnResumePressed()
    {
        Unpause();
    }
}

