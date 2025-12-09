using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float rotationSpeed = 100f;
    public int score = 0;
    public bool isAlive = true;

    public InputActionAsset inputActions;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    In inputSystem_Actions
    public InputActionReference move, look;

    public GameObject playerCanvas;

    public int playerIndex = 0; // 0 = first gamepad, 1 = second, etc.

    private Gamepad myGamepad;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        AssignGamepad();
    }

    void AssignGamepad()
    {
        var gamepads = Gamepad.all;
        if (playerIndex < gamepads.Count)
        {
            myGamepad = gamepads[playerIndex];
            Debug.Log($"Player {playerIndex + 1} assigned to {myGamepad.name}");
        }
        else
        {
            Debug.LogWarning($"No gamepad available for player {playerIndex + 1}");
        }
    }

    private void Update()
    {
        moveInput = move.action.ReadValue<Vector2>();
        lookInput = look.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        // Move forward/backward using stick Y
        Vector3 movement =
            (transform.forward * moveInput.y +
             transform.right * moveInput.x) * moveSpeed;

        rb.linearVelocity = movement;

        // Rotate using stick X
        float rotation = moveInput.x * rotationSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotation, 0f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isAlive) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Die();
            collision.gameObject.GetComponent<PlayerController>()?.Die();
        }

        if (collision.gameObject.CompareTag("Fruit"))
        {
            score += 1;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Plus"))
        {
            Destroy(collision.gameObject);
            GhostManager.Instance.MakeGhostsVulnerable(5f);
        }
    }

    public void Die()
    {
        isAlive = false;

        if (playerCanvas != null)
            playerCanvas.SetActive(true);

        gameObject.SetActive(false);
        //GameManager.Instance.CheckGameOver();
    }
}
