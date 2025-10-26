using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputAction moveAction, rotateAction, fireAction;

    Vector2 moveValue, rotateValue;

    float rotationSpeed;
    public float movementSpeed = 10.0f;

    [SerializeField] GameObject weaponPivot1;


    Vector3 angles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementSpeed = 10.0f;
        rotationSpeed = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        rotateValue = rotateAction.ReadValue<Vector2>();

        transform.Rotate(Vector3.up, rotateValue.x * rotationSpeed * Time.fixedDeltaTime);

        weaponPivot1.transform.Rotate(Vector3.right, -rotateValue.y * rotationSpeed * Time.fixedDeltaTime);

        angles = weaponPivot1.transform.localEulerAngles;

        if (angles.x < 300 && angles.x > 180)
        {
            weaponPivot1.transform.localRotation = Quaternion.Euler(300, 0, 0);
        }

        if (angles.x > 45 && angles.x < 180)
        {
            weaponPivot1.transform.localRotation = Quaternion.Euler(45, 0, 0);
        }

        if (fireAction.IsPressed())
        {
            BroadcastMessage("FireWeapon");
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(moveValue.x, 0, moveValue.y) * movementSpeed * Time.fixedDeltaTime);
    }

    private void OnEnable()
    {
        moveAction.Enable();
        rotateAction.Enable();
        fireAction.Enable();

    }

    private void OnDisable()
    {
        moveAction.Disable();
        rotateAction.Disable();
        fireAction.Disable();

    }

}
