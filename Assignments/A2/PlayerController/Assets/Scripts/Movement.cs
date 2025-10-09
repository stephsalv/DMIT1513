using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FrontLoaderMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 100f;

    [Header("Input Action")]
    public InputAction moveAction; // Vector2: x = rotate, y = forward/back

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
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        // Move forward/back
        Vector3 move = transform.forward * moveValue.y * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Rotate left/right
        float rotationY = moveValue.x * rotateSpeed * Time.fixedDeltaTime;
        Quaternion rotation = Quaternion.Euler(0f, rotationY, 0f);
        rb.MoveRotation(rb.rotation * rotation);
    }
}
