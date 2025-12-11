using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public bool PauseMenuInput {  get; private set; }

    private PlayerInput playerInput;

    private InputAction pauseMenuAction;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        playerInput = GetComponent<PlayerInput>();
        pauseMenuAction = playerInput.actions["Pause"];
    }

    private void Update()
    {
        PauseMenuInput = pauseMenuAction.WasPressedThisFrame();
    }

}
