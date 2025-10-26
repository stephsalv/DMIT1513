using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController1 : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction rotateAction;
    [SerializeField] private InputAction fireLeftAction;
    [SerializeField] private InputAction fireRightAction;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private float rotationSpeed = 100.0f;

    [Header("Weapon References")]
    [SerializeField] private GameObject leftWeaponPivot;
    [SerializeField] private GameObject rightWeaponPivot;

    private Vector2 moveValue;
    private Vector2 rotateValue;

    private void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        rotateValue = rotateAction.ReadValue<Vector2>();

        // --- Player rotation ---
        transform.Rotate(Vector3.up, rotateValue.x * rotationSpeed * Time.deltaTime);

        // --- Weapon pitch (vertical aim) ---
        if (leftWeaponPivot != null)
            AdjustWeaponPitch(leftWeaponPivot, -rotateValue.y);

        if (rightWeaponPivot != null)
            AdjustWeaponPitch(rightWeaponPivot, -rotateValue.y);

        // --- Firing ---
        if (fireLeftAction.IsPressed())
        {
            leftWeaponPivot?.BroadcastMessage("FireWeapon", SendMessageOptions.DontRequireReceiver);
        }

        if (fireRightAction.IsPressed())
        {
            rightWeaponPivot?.BroadcastMessage("FireWeapon", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void FixedUpdate()
    {
        // --- Movement ---
        Vector3 move = new Vector3(moveValue.x, 0, moveValue.y) * movementSpeed * Time.fixedDeltaTime;
        transform.Translate(move, Space.Self);
    }

    private void AdjustWeaponPitch(GameObject pivot, float verticalInput)
    {
        float pitch = pivot.transform.localEulerAngles.x;
        pitch = (pitch > 180) ? pitch - 360 : pitch; // Convert to -180..180 range
        pitch = Mathf.Clamp(pitch + verticalInput * rotationSpeed * Time.deltaTime, -60f, 45f);
        pivot.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    private void OnEnable()
    {
        moveAction.Enable();
        rotateAction.Enable();
        fireLeftAction.Enable();
        fireRightAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        rotateAction.Disable();
        fireLeftAction.Disable();
        fireRightAction.Disable();
    }
}
