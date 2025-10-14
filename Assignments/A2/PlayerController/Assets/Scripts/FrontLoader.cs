using UnityEngine;
using UnityEngine.InputSystem;

public class TruckControl : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] float rotationSpeed = 100.0f;

    [SerializeField] Transform arm;
    [SerializeField] float armSpeed = 30.0f;
    [SerializeField] float armMinAngle = 25.0f;
    [SerializeField] float armMaxAngle = -60.0f;

    [SerializeField] Transform bucket;
    [SerializeField] float bucketSpeed = 40.0f;
    [SerializeField] float bucketMinAngle = 70.0f;
    [SerializeField] float bucketMaxAngle = -40.0f;

    private float armAngle = 0f;
    private float bucketAngle = 0f;

    Rigidbody rbody;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Movement();
        ArmRotation();
        BucketRotation();
    }

    void Movement()
    {
        // Move forward/backward
        float moveInput = 0f;
        if (Keyboard.current.wKey.isPressed) moveInput = 1f;
        else if (Keyboard.current.sKey.isPressed) moveInput = -1f;

        // Rotate left/right
        float rotateInput = 0f;
        if (Keyboard.current.aKey.isPressed) rotateInput = -1f;
        else if (Keyboard.current.dKey.isPressed) rotateInput = 1f;

        // Apply translation and rotation
        transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, rotateInput * rotationSpeed * Time.deltaTime);
    }

    void ArmRotation()
    {
        if (arm == null) return;

        float armInput = 0f;
        if (Keyboard.current.uKey.isPressed) armInput = 1f;
        else if (Keyboard.current.jKey.isPressed) armInput = -1f;

        armAngle += armInput * armSpeed * Time.deltaTime;

        armAngle = Mathf.Clamp(armAngle, armMinAngle, armMaxAngle);

        arm.localRotation = Quaternion.Euler(armAngle, 0f, 0f);
    }

    void BucketRotation()
    {
        if (bucket == null) return;

        float bucketInput = 0f;
        if (Keyboard.current.iKey.isPressed) bucketInput = 1f;
        else if (Keyboard.current.kKey.isPressed) bucketInput = -1f;

        bucketAngle += bucketInput * bucketSpeed * Time.deltaTime;

        bucketAngle = Mathf.Clamp(bucketAngle, bucketMinAngle, bucketMaxAngle);

        bucket.localRotation = Quaternion.Euler(bucketAngle, 0f, 0f);
    }
}

