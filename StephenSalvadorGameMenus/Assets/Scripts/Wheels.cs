using UnityEngine;
using UnityEngine.InputSystem;

public class WheelSteeringVisualizer : MonoBehaviour
{
    [SerializeField] private Rigidbody carRigidbody;

    [Header("Wheel Meshes")]
    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private Transform rearLeftWheel;
    [SerializeField] private Transform rearRightWheel;

    [Header("Steering Settings")]
    [SerializeField] private float maxSteerAngle = 45f;
    [SerializeField] private float steerSpeed = 10f; // how fast wheels visually rotate
    [SerializeField] private bool invertSteering = false;

    [Header("Wheel Spin Settings")]
    [SerializeField] private float wheelRadius = 0.35f; // in meters
    [SerializeField] private float spinMultiplier = 90f; // visual speed multiplier
    private float frontLeftSpin = 0f;
    private float frontRightSpin = 0f;
    private float rearLeftSpin = 0f;
    private float rearRightSpin = 0f;

    [Header("Input Settings")]
    [SerializeField] private int gamepadIndex = 0;
    private float steerInput = 0f;
    private float currentSteer = 0f;
    private float moveInput = 0f;

    void Update()
    {
        HandleInput();
        HandleSteering();
        HandleWheelSpin();
    }

    void HandleInput()
    {
        // Reset inputs
        steerInput = 0f;
        moveInput = 0f;

        // Keyboard
        if (Keyboard.current.aKey.isPressed) steerInput = -1f;
        else if (Keyboard.current.dKey.isPressed) steerInput = 1f;

        if (Keyboard.current.wKey.isPressed) moveInput = 1f;
        else if (Keyboard.current.sKey.isPressed) moveInput = -1f;

        // Gamepad
        if (Gamepad.all.Count > gamepadIndex)
        {
            steerInput += Gamepad.all[gamepadIndex].leftStick.x.ReadValue();
            moveInput += Gamepad.all[gamepadIndex].rightTrigger.ReadValue(); // accelerate
            moveInput -= Gamepad.all[gamepadIndex].leftTrigger.ReadValue();  // brake/reverse
        }

        steerInput = Mathf.Clamp(steerInput, -1f, 1f);
        moveInput = Mathf.Clamp(moveInput, -1f, 1f);

        if (invertSteering) steerInput *= -1f;
    }

    void HandleSteering()
    {
        currentSteer = Mathf.MoveTowards(currentSteer, steerInput, steerSpeed * Time.deltaTime);

        // Apply steering without affecting spin
        frontLeftWheel.localRotation = Quaternion.Euler(frontLeftSpin, currentSteer * maxSteerAngle, 0f);
        frontRightWheel.localRotation = Quaternion.Euler(frontRightSpin, currentSteer * maxSteerAngle, 0f);
    }

    void HandleWheelSpin()
    {
        float spinDelta;

        if (carRigidbody != null)
        {
            float forwardSpeed = Vector3.Dot(carRigidbody.linearVelocity, carRigidbody.transform.forward);
            float wheelCircumference = 2f * Mathf.PI * wheelRadius;
            spinDelta = (forwardSpeed / wheelCircumference) * 360f * Time.deltaTime;
        }
        else
        {
            float wheelCircumference = 2f * Mathf.PI * wheelRadius;
            spinDelta = (moveInput * spinMultiplier) * (360f / wheelCircumference) * Time.deltaTime;
        }

        // Update cumulative spin
        frontLeftSpin += spinDelta;
        frontRightSpin += spinDelta;
        rearLeftSpin += spinDelta;
        rearRightSpin += spinDelta;

        // Apply spin to rear wheels (X-axis) � front wheels already combined in HandleSteering
        rearLeftWheel.localRotation = Quaternion.Euler(rearLeftSpin, 0f, 0f);
        rearRightWheel.localRotation = Quaternion.Euler(rearRightSpin, 0f, 0f);
    }
}

