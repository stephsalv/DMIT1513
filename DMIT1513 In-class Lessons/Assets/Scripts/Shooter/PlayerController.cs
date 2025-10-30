using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
public class PlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction rotateAction;
    [SerializeField] private InputAction fireAction;
    [SerializeField] private InputAction jumpAction;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private GameObject weaponPivot;
    [SerializeField] private GameObject firstPerson;
    [SerializeField] private GameObject thirdPerson;
    [SerializeField] private GameObject playerCam;

    public static bool dialogue = false;
    private Vector2 moveValue;
    private Vector2 rotateValue;
    private bool firstPersonPerspective = true;
    private Vector3 angles;

    private Vector3 velocity;
    private bool isGrounded = true; // simple grounded check

    void Start()
    {
        playerCam.transform.localPosition = firstPerson.transform.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (dialogue)
        {
            return;
        }

        moveValue = moveAction.ReadValue<Vector2>();
        rotateValue = rotateAction.ReadValue<Vector2>();
        transform.Rotate(Vector3.up, rotateValue.x * rotationSpeed * Time.fixedDeltaTime);
        weaponPivot.transform.Rotate(Vector3.right, -rotateValue.y * rotationSpeed * Time.fixedDeltaTime);

        angles = weaponPivot.transform.localEulerAngles;
        if (angles.x < 300 && angles.x > 180)
        {
            weaponPivot.transform.localRotation = Quaternion.Euler(300, 0, 0);
        }
        if (angles.x > 45 && angles.x < 180)
        {
            weaponPivot.transform.localRotation = Quaternion.Euler(45, 0, 0);
        }
        if (fireAction.IsPressed())
        {
            BroadcastMessage("FireWeapon");
        }
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            firstPersonPerspective = !firstPersonPerspective;
            if (firstPersonPerspective)
            {
                playerCam.transform.localPosition = firstPerson.transform.localPosition;
            }
            else
            {
                playerCam.transform.localPosition = thirdPerson.transform.localPosition;
            }
        }
        // Jump input
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
        }
        // Gravity
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            isGrounded = false;
        }
    }
    private void FixedUpdate()
    {
        if (!PlayerController.dialogue)
        {
            transform.Translate(new Vector3(moveValue.x, 0, moveValue.y) * movementSpeed * Time.fixedDeltaTime);
        }
        if (dialogue)
        {
            return;
        }
        // Vertical movement (jump + gravity)
        transform.Translate(Vector3.up * velocity.y * Time.fixedDeltaTime, Space.World);

        // Simple grounded check
        if (transform.position.y <= 0f) // assuming 0 is ground level
        {
            isGrounded = true;
            velocity.y = 0f;
            Vector3 pos = transform.position;
            pos.y = 0f;
            transform.position = pos;
        }
        else
        {
            isGrounded = false;
        }
    }
    void MyInput()
    {
        // Get horizontal and vertical input
        moveValue.x = Input.GetAxis("Horizontal"); // A/D or Left/Right
        moveValue.y = Input.GetAxis("Vertical"); // W/S or Up/Down
    }
    private void OnEnable()
    {
        moveAction.Enable();
        rotateAction.Enable();
        fireAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        rotateAction.Disable();
        fireAction.Disable();
        jumpAction.Disable();
    }
}