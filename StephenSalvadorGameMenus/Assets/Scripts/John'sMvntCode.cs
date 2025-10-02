using UnityEngine;
using UnityEngine.InputSystem;

public class CarControl : MonoBehaviour
{
    [SerializeField] float decelerationRate = 5f;
    [SerializeField] GameObject car;
    [SerializeField] int gamepadIndex;

    float currentSpeed = 0f;
    float inputForward = 0f;
    float inputTurn = 0f;

    void FixedUpdate()
    {
        // Get current settings from the Singleton
        float maxSpeed = CarSettingsManager.Instance.maxSpeed;
        float accelSpeed = CarSettingsManager.Instance.forwardAccel;
        float rotationSpeed = CarSettingsManager.Instance.turnStrength;

        // Reset input
        inputForward = 0f;
        inputTurn = 0f;

        // Keyboard Input
        if (Keyboard.current.wKey.isPressed) inputForward = 1f;
        if (Keyboard.current.sKey.isPressed) inputForward = -1f;
        if (Keyboard.current.aKey.isPressed) inputTurn = -1f;
        if (Keyboard.current.dKey.isPressed) inputTurn = 1f;

        // Gamepad Input
        if (Gamepad.all.Count > gamepadIndex)
        {
            var gamepad = Gamepad.all[gamepadIndex];
            inputForward += gamepad.leftStick.y.ReadValue(); // forward/backward
            inputTurn += gamepad.leftStick.x.ReadValue();    // left/right
        }

        // Apply acceleration
        if (Mathf.Abs(inputForward) > 0.1f)
        {
            currentSpeed += inputForward * accelSpeed * Time.fixedDeltaTime;
        }
        else
        {
            // Apply deceleration
            if (currentSpeed > 0)
                currentSpeed -= decelerationRate * Time.fixedDeltaTime;
            else if (currentSpeed < 0)
                currentSpeed += decelerationRate * Time.fixedDeltaTime;

            // Clamp near-zero to zero
            if (Mathf.Abs(currentSpeed) < 0.1f)
                currentSpeed = 0f;
        }

        // Clamp speed
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // Move and rotate
        transform.Translate(Vector3.forward * currentSpeed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up * inputTurn * rotationSpeed * Time.fixedDeltaTime);
    }
}