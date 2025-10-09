using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Hoverboard : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveForce = 1000f;
    public float rotateTorque = 500f;

    [Header("Hover Settings")]
    public float hoverHeight = 1f;           // Target hover height
    public float hoverForce = 1000f;         // Force applied to maintain hover
    public float hoverDamping = 0.5f;        // Smoothness of hover
    public float maxHoverDistance = 5f;      // Max distance to check ground

    [Header("Input Actions")]
    public InputAction moveAction;  // Vector2: x = rotation, y = forward/back

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleHover();
    }

    private void HandleMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        // Forward/backward force
        Vector3 forwardForce = transform.forward * input.y * moveForce * Time.fixedDeltaTime;
        rb.AddForce(forwardForce);

        // Rotate vehicle (yaw)
        Vector3 torque = Vector3.up * input.x * rotateTorque * Time.fixedDeltaTime;
        rb.AddTorque(torque);
    }

    private void HandleHover()
    {
        // Raycast downward from multiple points on the vehicle
        Vector3[] hoverPoints = new Vector3[]
        {
            transform.position,                             // center
            transform.position + transform.forward,        // front
            transform.position - transform.forward,        // back
            transform.position + transform.right,          // right
            transform.position - transform.right           // left
        };

        float closestDistance = maxHoverDistance;

        foreach (Vector3 point in hoverPoints)
        {
            if (Physics.Raycast(point, Vector3.down, out RaycastHit hit, maxHoverDistance))
            {
                if (hit.distance < closestDistance)
                    closestDistance = hit.distance;
            }
        }

        float heightError = hoverHeight - closestDistance;

        // Apply upward force proportional to height difference
        rb.AddForce(Vector3.up * heightError * hoverForce * Time.fixedDeltaTime, ForceMode.Acceleration);

        // Optional: damping to reduce oscillations
        rb.AddForce(-rb.linearVelocity.y * Vector3.up * hoverDamping, ForceMode.Acceleration);
    }

}
