using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction rotateAction;
    [SerializeField] private InputAction fireAction;
    [SerializeField] private InputAction jumpAction;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float rotationSpeed = 80f;


    [Header("References")]
    [SerializeField] private GameObject weaponPivot;
    [SerializeField] private GameObject firstPerson;
    [SerializeField] private GameObject thirdPerson;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private GameObject sideView;


    public static bool dialogue = false;
    private Vector2 moveValue;
    private Vector2 rotateValue;
    private bool firstPersonPerspective = true;
    private Vector3 angles;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundHeight = 0f;

    private float verticalVelocity = 0f;
    private bool isGrounded = true;

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
            // Move camera to side view
            playerCam.transform.localPosition = sideView.transform.localPosition;
            // Make the camera face the player's left side (local -X direction)
            playerCam.transform.localRotation = Quaternion.LookRotation(-transform.right, Vector3.up);
            return;
        }
        else
        {
            // Restore camera position and rotation when not in dialogue
            if (firstPersonPerspective)
            {
                playerCam.transform.localPosition = firstPerson.transform.localPosition;
                playerCam.transform.localRotation = firstPerson.transform.localRotation;
            }
            else
            {
                playerCam.transform.localPosition = thirdPerson.transform.localPosition;
                playerCam.transform.localRotation = thirdPerson.transform.localRotation;
            }
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
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
        // Skip movement during dialogue
        if (dialogue) return;

        // Jump input
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            verticalVelocity = jumpForce;
            isGrounded = false;
        }

        // Apply gravity
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Apply vertical movement
        transform.Translate(Vector3.up * verticalVelocity * Time.deltaTime, Space.World);

        // Simple ground check (flat ground)
        if (transform.position.y <= groundHeight)
        {
            Vector3 pos = transform.position;
            pos.y = groundHeight;
            transform.position = pos;
            verticalVelocity = 0f;
            isGrounded = true;
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