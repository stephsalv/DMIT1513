using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Player Info")]
    public float moveSpeed = 15f;
    public float rotationSpeed = 100f;
    public int score = 0;
    public bool isAlive = true;

    [Header("Input Info")]
    public InputActionAsset inputActions;
    public InputActionReference move, look;
    public int playerIndex = 0; // 0 = first gamepad, 1 = second, etc.

    [Header("Canvas")]
    public GameObject playerDiedCanvas;
    public GameObject playerWonCanvas;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip eatClip;
    public AudioClip switchClip;

    [Header("UI")]
    [SerializeField] public TMP_Text scoreText;

    public Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Gamepad myGamepad;
    private Joystick myJoystick;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        AssignInputDevice();
    }

    void AssignInputDevice()
    {
        // Prefer gamepad if available
        var gamepads = Gamepad.all;
        if (playerIndex < gamepads.Count)
        {
            myGamepad = gamepads[playerIndex];
            Debug.Log($"Player {playerIndex + 1} assigned to Gamepad {myGamepad.name}");
        }
        else
        {
            // If no gamepad, check for joystick
            var joysticks = Joystick.all;
            int joystickIndex = playerIndex - gamepads.Count;
            if (joystickIndex < joysticks.Count)
            {
                myJoystick = joysticks[joystickIndex];
                Debug.Log($"Player {playerIndex + 1} assigned to Joystick {myJoystick.name}");
            }
            else
            {
                Debug.LogWarning($"No input device available for player {playerIndex + 1}");
            }
        }
    }

    void Update()
    {
        if (!isAlive) return;

        // Read input from assigned device
        if (myGamepad != null)
        {
            moveInput = myGamepad.leftStick.ReadValue();
            lookInput = myGamepad.rightStick.ReadValue();
        }
        else if (myJoystick != null)
        {
            // Most joysticks use stick.x / stick.y for movement
            moveInput = new Vector2(myJoystick.stick.x.ReadValue(), myJoystick.stick.y.ReadValue());
            // For simplicity, lookInput can use the same as moveInput
            lookInput = moveInput;
        }
        UpdateScoreUI();
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
            //collision.gameObject.GetComponent<PlayerController>()?.Die();
        }

        if (collision.gameObject.CompareTag("Fruit"))
        {
            score += 1;
            Destroy(collision.gameObject);

            UpdateScoreUI();

            GameManager.Instance.CheckScoreWinner(this); // <-- ADD THIS

            if (audioSource != null && eatClip != null)
                audioSource.PlayOneShot(eatClip);
        }

        if (collision.gameObject.CompareTag("Plus")) // or "PowerUp"
        {
            Destroy(collision.gameObject);
            GhostManager.Instance.MakeGhostsVulnerable(5f);

            if (audioSource != null && switchClip != null)
                audioSource.PlayOneShot(switchClip);
        }
    }
    public void Die()
    {
        if (!isAlive) return;

        isAlive = false;

        if (playerDiedCanvas != null)
            playerDiedCanvas.SetActive(true);

        // Notify GameManager
        GameManager.Instance.PlayerDied(this);

        gameObject.SetActive(false);
        audioSource.Stop();

        GameManager.Instance.GameOver();
    }
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "" + score;
    }
}
