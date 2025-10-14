using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[RequireComponent(typeof(Rigidbody))]
public class TruckControl : MonoBehaviour
{
    [SerializeField] float movementSpeed = 3.0f;
    [SerializeField] float rotationSpeed = 100.0f;

    [SerializeField] GameObject arm;
    [SerializeField] GameObject bucket;

    [SerializeField] InputActionAsset inputActionsAsset;
    [SerializeField] string actionMapName = "Player";
    [SerializeField] int gamepadIndex = 0;

    private InputAction moveAction;
    private InputAction armRotateAction;
    private InputAction bucketRotateAction;

    private Rigidbody rbody;

    private Vector2 moveValue;
    private float armRotateValue;
    private float bucketRotateValue;

    void Awake()
    {
        rbody = GetComponent<Rigidbody>();

        // Find action map & actions
        var actionMap = inputActionsAsset.FindActionMap(actionMapName, true);

        moveAction = actionMap.FindAction("Move", true);
        armRotateAction = actionMap.FindAction("TurretRotate", true);
        bucketRotateAction = actionMap.FindAction("BucketRotate", false); // optional

        moveAction.Enable();
        armRotateAction.Enable();
        bucketRotateAction?.Enable();

        // Optional: Assign to specific gamepad if available
        if (Gamepad.all.Count > gamepadIndex)
        {
            var gamepad = Gamepad.all[gamepadIndex];
            InputUser.PerformPairingWithDevice(gamepad);
        }
    }

    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        armRotateValue = armRotateAction.ReadValue<float>();
        bucketRotateValue = bucketRotateAction != null ? bucketRotateAction.ReadValue<float>() : 0f;
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = transform.forward * moveValue.y * movementSpeed * Time.fixedDeltaTime;
        Vector3 newPosition = rbody.position + moveDirection;
        rbody.MovePosition(newPosition);

        Quaternion turnRotation = Quaternion.Euler(0f, moveValue.x * rotationSpeed * Time.fixedDeltaTime, 0f);
        rbody.MoveRotation(rbody.rotation * turnRotation);

        if (arm != null)
        {
            arm.transform.Rotate(Vector3.up, armRotateValue * rotationSpeed * Time.fixedDeltaTime, Space.Self);
        }

        if (bucket != null)
        {
            bucket.transform.Rotate(Vector3.right, bucketRotateValue * rotationSpeed * Time.fixedDeltaTime, Space.Self);
        }
    }
}
