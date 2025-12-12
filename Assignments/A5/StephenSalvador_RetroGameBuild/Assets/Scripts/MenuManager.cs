using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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

    [Header("Input (Assign Pause Action Here)")]
    public InputActionReference pauseActionRef;  // <-- FIX

    [Header("UI Buttons")]
    public GameObject resumeButton;

    private bool isPaused = false;

    private void OnEnable()
    {
        pauseActionRef.action.Enable();
        pauseActionRef.action.performed += OnPausePressed;
    }

    private void OnDisable()
    {
        pauseActionRef.action.performed -= OnPausePressed;
        pauseActionRef.action.Disable();
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        if (isPaused)
            Unpause();
        else
            Pause();
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (player != null) player.enabled = false;
        if (ghost != null) ghost.enabled = false;

        pauseMenu.SetActive(true);

        // IMPORTANT FIX — reset selection first
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);

        if (audioSource && pauseClip)
            audioSource.PlayOneShot(pauseClip);
    }


    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (player != null) player.enabled = true;
        if (ghost != null) ghost.enabled = true;

        pauseMenu.SetActive(false);

        if (audioSource && pauseClip)
            audioSource.PlayOneShot(pauseClip);
    }

    public void OnResumePressed() => Unpause();

    public void OnExitPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
