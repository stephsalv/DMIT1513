using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class HoverMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveForce = 500f;
    [SerializeField] float turnTorque = 300f;

    [Header("Hover Settings")]
    [SerializeField] float hoverHeight = 1f;
    [SerializeField] float hoverForce = 100f;
    [SerializeField] float hoverDamp = 0.5f;
    [SerializeField] Transform[] hoverPoints;

    [Header("Input")]
    [SerializeField] InputAction moveAction;

    Rigidbody rbody;
    Vector2 moveValue;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        moveAction.Enable();
    }

    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        ApplyHoverForces();
        ApplyMovementForces();
    }

    void ApplyHoverForces()
    {
        bool allTooFar = true;
        bool anyClose = false;

        foreach (Transform point in hoverPoints)
        {
            if (Physics.Raycast(point.position, Vector3.down, out RaycastHit hit, hoverHeight * 2f))
            {
                float distance = hit.distance;
                float heightError = hoverHeight - distance;

                // Basic spring force
                float upwardSpeed = Vector3.Dot(rbody.GetPointVelocity(point.position), Vector3.up);
                float lift = (heightError * hoverForce) - (upwardSpeed * hoverDamp);

                rbody.AddForceAtPosition(Vector3.up * lift, point.position, ForceMode.Force);

                // Track if ground is near or far
                if (distance < hoverHeight) anyClose = true;
                if (distance <= hoverHeight * 1.5f) allTooFar = false;
            }
            else
            {
                // No ground detected within ray range
                allTooFar = true;
            }
        }

        // If all points are far from ground, apply a soft downward force to descend
        if (allTooFar)
        {
            rbody.AddForce(Vector3.down * hoverForce * 0.5f, ForceMode.Acceleration);
        }

        // If any are too close, hover points will self-correct via spring forces
    }

    void ApplyMovementForces()
    {
        // Forward/backward
        Vector3 forwardForce = transform.forward * moveValue.y * moveForce * Time.fixedDeltaTime;
        rbody.AddForce(forwardForce, ForceMode.Force);

        // Rotation (yaw)
        float turn = moveValue.x * turnTorque * Time.fixedDeltaTime;
        rbody.AddTorque(Vector3.up * turn, ForceMode.Force);
    }

    void OnDrawGizmosSelected()
    {
        if (hoverPoints == null) return;

        Gizmos.color = Color.cyan;
        foreach (var point in hoverPoints)
        {
            if (point == null) continue;
            Gizmos.DrawLine(point.position, point.position + Vector3.down * hoverHeight);
            Gizmos.DrawWireSphere(point.position + Vector3.down * hoverHeight, 0.05f);
        }
    }
}

