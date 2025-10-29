using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction rotateAction;
    [SerializeField] private InputAction fireAction;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 100f;

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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        moveValue.y = Input.GetAxis("Vertical");   // W/S or Up/Down
    }

    private void OnEnable()
    {
        moveAction.Enable();
        rotateAction.Enable();
        fireAction.Enable();
        //fireAction2.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        rotateAction.Disable();
        fireAction.Disable();
        //fireAction2.Disable();
    }
}
