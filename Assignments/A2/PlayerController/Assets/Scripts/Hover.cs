using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class HoverScript : MonoBehaviour
{
    [Header("Hover Settings")]
    [SerializeField] float hoverHeight = 1.0f;
    [SerializeField] float hoverForce = 100.0f;
    [SerializeField] float hoverDamp = 5.0f;

    [Header("Movement Settings")]
    [SerializeField] float moveForce = 50.0f;
    [SerializeField] float turnTorque = 30.0f;

    [Header("Hover Stability")]
    [SerializeField] Transform[] hoverPoints;

    Rigidbody rb;
    float moveInput;
    float turnInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;  // Let physics handle downward pull
        rb.linearDamping = 1f;
        rb.angularDamping = 2f;
    }

    void Update()
    {
        // Read input
        moveInput = 0f;
        turnInput = 0f;

        if (Keyboard.current.upArrowKey.isPressed) moveInput = 1f;
        else if (Keyboard.current.downArrowKey.isPressed) moveInput = -1f;

        if (Keyboard.current.leftArrowKey.isPressed) turnInput = -1f;
        else if (Keyboard.current.rightArrowKey.isPressed) turnInput = 1f;
    }

    void FixedUpdate()
    {
        // Apply hover force at each hover point (for stability)
        foreach (var point in hoverPoints)
        {
            RaycastHit hit;
            if (Physics.Raycast(point.position, Vector3.down, out hit, hoverHeight * 2f))
            {
                float heightError = hoverHeight - hit.distance;
                float upwardSpeed = rb.GetPointVelocity(point.position).y;
                float lift = (heightError * hoverForce) - (upwardSpeed * hoverDamp);

                rb.AddForceAtPosition(Vector3.up * lift, point.position, ForceMode.Acceleration);
            }
            else
            {
                // Apply downward pull if no ground detected
                rb.AddForceAtPosition(Vector3.down * hoverForce * 0.5f, point.position, ForceMode.Acceleration);
            }
        }

        // Movement: apply forward/backward force
        Vector3 forwardForce = transform.forward * moveInput * moveForce;
        rb.AddForce(forwardForce, ForceMode.Force);

        // Turning: apply torque
        rb.AddTorque(Vector3.up * turnInput * turnTorque, ForceMode.Force);
    }
}


