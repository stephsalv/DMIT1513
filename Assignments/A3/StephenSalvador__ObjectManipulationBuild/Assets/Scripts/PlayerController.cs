using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction rotateAction;
    [SerializeField] private InputAction fireAction;
    [SerializeField] private InputAction jumpAction;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float rotationSpeed = 80f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundHeight = 0f;

    [Header("Cameras")]
    [SerializeField] private GameObject weaponPivot;
    [SerializeField] private GameObject firstPerson;
    [SerializeField] private GameObject thirdPerson;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private GameObject sideCam;

    [Header("Weapons")]
    [SerializeField] private List<WeaponData> weapons = new List<WeaponData>();
    [SerializeField] private GameObject weaponPickupPrefab;

    private int activeWeaponIndex = 0;
    private GameObject currentWeaponInstance;
    private float verticalVelocity = 0f;
    private bool isGrounded = true;
    private bool firstPersonPerspective = true;
    private Vector2 moveValue;
    private Vector2 rotateValue;

    public static bool dialogue = false;

    void Start()
    {
        playerCam.transform.localPosition = firstPerson.transform.localPosition;
        EquipWeapon(activeWeaponIndex);
    }

    void Update()
    {
        HandleCursor();

        if (dialogue) { SetDialogueCamera(true); return; }
        else { SetDialogueCamera(false); }

        HandleCameraSwitch();
        HandleMovementInput();
        HandleRotation();
        HandleJumpAndGravity();
        HandleFire();
        HandleWeaponSwap();
        HandleWeaponDrop();
    }

    private void FixedUpdate()
    {
        if (!dialogue)
        {
            Vector3 move = new Vector3(moveValue.x, 0, moveValue.y) * movementSpeed * Time.fixedDeltaTime;
            transform.Translate(move, Space.Self);
        }
    }

    private void HandleCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetDialogueCamera(bool dialogueActive)
    {
        sideCam.SetActive(dialogueActive);
        playerCam.SetActive(!dialogueActive);
    }

    private void HandleCameraSwitch()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            firstPersonPerspective = !firstPersonPerspective;
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
    }

    private void HandleMovementInput()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        rotateValue = rotateAction.ReadValue<Vector2>();
    }

    private void HandleRotation()
    {
        transform.Rotate(Vector3.up, rotateValue.x * rotationSpeed * Time.fixedDeltaTime);
        weaponPivot.transform.Rotate(Vector3.right, -rotateValue.y * rotationSpeed * Time.fixedDeltaTime);

        Vector3 angles = weaponPivot.transform.localEulerAngles;
        if (angles.x < 300 && angles.x > 180) weaponPivot.transform.localRotation = Quaternion.Euler(300, 0, 0);
        if (angles.x > 45 && angles.x < 180) weaponPivot.transform.localRotation = Quaternion.Euler(45, 0, 0);
    }

    private void HandleJumpAndGravity()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            verticalVelocity = jumpForce;
            isGrounded = false;
        }

        if (!isGrounded)
            verticalVelocity += gravity * Time.deltaTime;

        transform.Translate(Vector3.up * verticalVelocity * Time.deltaTime, Space.World);

        if (transform.position.y <= groundHeight)
        {
            Vector3 pos = transform.position;
            pos.y = groundHeight;
            transform.position = pos;
            verticalVelocity = 0f;
            isGrounded = true;
        }
    }

    private void HandleFire()
    {
        if (fireAction.IsPressed() && currentWeaponInstance != null)
        {
            currentWeaponInstance.GetComponent<Weapon>()?.FireWeapon();
        }
    }

    private void HandleWeaponSwap()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame && weapons.Count > 1)
        {
            activeWeaponIndex++;
            if (activeWeaponIndex >= weapons.Count) activeWeaponIndex = 0;
            EquipWeapon(activeWeaponIndex);
        }
    }

    private void HandleWeaponDrop()
    {
        if (Keyboard.current.gKey.wasPressedThisFrame && weapons.Count > 0)
        {
            DropWeapon(activeWeaponIndex);
        }
    }

    public void PickupWeapon(WeaponData weapon)
    {
        if (!weapons.Contains(weapon))
        {
            weapons.Add(weapon);
            EquipWeapon(weapons.Count - 1);
        }
    }

    public void DropWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count) return;

        WeaponData droppedWeapon = weapons[index];
        weapons.RemoveAt(index);

        if (weaponPickupPrefab != null)
        {
            GameObject pickup = Instantiate(weaponPickupPrefab, transform.position + transform.forward, Quaternion.identity);
            pickup.GetComponent<WeaponPickup>().weaponData = droppedWeapon;
        }

        if (activeWeaponIndex >= weapons.Count) activeWeaponIndex = weapons.Count - 1;
        EquipWeapon(activeWeaponIndex);
    }

    private void EquipWeapon(int index)
    {
        if (currentWeaponInstance != null)
            Destroy(currentWeaponInstance);

        if (weapons.Count == 0) return;

        activeWeaponIndex = index;
        WeaponData weapon = weapons[index];

        if (weapon.weaponPrefab != null)
        {
            currentWeaponInstance = Instantiate(weapon.weaponPrefab, weaponPivot.transform);
            currentWeaponInstance.transform.localPosition = Vector3.zero;
            currentWeaponInstance.transform.localRotation = Quaternion.identity;

            // Assign WeaponData to Weapon script
            Weapon weaponComp = currentWeaponInstance.GetComponent<Weapon>();
            if (weaponComp != null)
                weaponComp.weaponData = weapon;
        }
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