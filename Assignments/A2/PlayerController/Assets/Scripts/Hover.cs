using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Hovercraft : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardAccel = 30f;
    public float reverseAccel = 20f;
    public float turnStrength = 50f;
    public float maxSpeed = 10f;

    [Header("Hover Settings")]
    public float hoverHeight = 1f;
    public float hoverForce = 100f;
    public float hoverDamping = 5f;

    [Header("Input Settings")]
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    private Rigidbody rb;
    private float speedInput;
    private float turnInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Handle input
        speedInput = 0f;
        if (Input.GetKey(forwardKey)) speedInput += 1f;
        if (Input.GetKey(backwardKey)) speedInput -= 1f;

        turnInput = 0f;
        if (Input.GetKey(leftKey)) turnInput -= 1f;
        if (Input.GetKey(rightKey)) turnInput += 1f;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Forward/backward
        float accel = speedInput >= 0 ? forwardAccel : reverseAccel;
        rb.AddForce(transform.forward * speedInput * accel, ForceMode.Acceleration);

        // Clamp horizontal speed
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
        }

        // Turn
        if (Mathf.Abs(speedInput) > 0.1f)
        {
            Quaternion turnOffset = Quaternion.Euler(0f, turnInput * turnStrength * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnOffset);
        }
    }
}
