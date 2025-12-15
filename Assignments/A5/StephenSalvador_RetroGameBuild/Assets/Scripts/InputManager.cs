using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public bool PauseMenuInput { get; private set; }

    private PlayerInput playerInput;
    private InputAction pauseMenuAction;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        playerInput = GetComponent<PlayerInput>();

        pauseMenuAction = playerInput.actions["Pause"];

        // ✅ subscribe ONCE
        pauseMenuAction.performed += OnPausePerformed;
    }

    private void OnDestroy()
    {
        if (pauseMenuAction != null)
            pauseMenuAction.performed -= OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        // 🚫 Ignore keyboard & mouse
        if (context.control.device is Keyboard || context.control.device is Mouse)
            return;

        // 🎮 Accept gamepad / joystick
        if (context.control.device is Gamepad || context.control.device is Joystick)
        {
            PauseMenuInput = true;
        }
    }

    private void LateUpdate()
    {
        // consume input so it triggers once
        PauseMenuInput = false;
    }
}
