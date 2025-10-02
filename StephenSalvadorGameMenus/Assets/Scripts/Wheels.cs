using UnityEngine;
using UnityEngine.InputSystem;

public class WheelSteeringVisualizer : MonoBehaviour
{
    [Header("Wheel Meshes")]
    [SerializeField] Transform frontLeftWheel;
    [SerializeField] Transform frontRightWheel;

    [Header("Steering Settings")]
    [SerializeField] float maxSteerAngle = 50f;
    [SerializeField] float steerSmoothness = 5f;
    [SerializeField] int gamepadIndex = 0;

    float steerInput = 0f;

    void Update()
    {
        // Reset input
        steerInput = 0f;

        // Keyboard input
        if (Keyboard.current.aKey.isPressed) steerInput = -1f;
        if (Keyboard.current.dKey.isPressed) steerInput = 1f;

        // Gamepad input
        if (Gamepad.all.Count > gamepadIndex)
        {
            steerInput += Gamepad.all[gamepadIndex].leftStick.x.ReadValue();
        }

        // Clamp input
        steerInput = Mathf.Clamp(steerInput, -1f, 1f);

        // Apply rotation to wheel meshes
        Quaternion targetRotation = Quaternion.Euler(0f, steerInput * maxSteerAngle, 0f);
        frontLeftWheel.localRotation = Quaternion.Lerp(frontLeftWheel.localRotation, targetRotation, Time.deltaTime * steerSmoothness);
        frontRightWheel.localRotation = Quaternion.Lerp(frontRightWheel.localRotation, targetRotation, Time.deltaTime * steerSmoothness);
    }
}
