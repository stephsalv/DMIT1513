using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int score = 0;
    public bool isAlive = true;

    private Rigidbody rb;
    private Vector2 moveInput;

    public InputActionReference move;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveInput = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        // Movement
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        rb.linearVelocity = movement;

        // Rotate to movement direction
        if (movement.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isAlive) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Die();
            collision.gameObject.GetComponent<PlayerController>()?.Die();
        }

        if (collision.gameObject.CompareTag("Ghost"))
        {
            var ghost = collision.gameObject.GetComponent<Ghost>();
            if (ghost.isVulnerable)
            {
                score += 5;
                ghost.Die();
            }
            else
            {
                Die();
            }
        }

        if (collision.gameObject.CompareTag("Fruit"))
        {
            score += 1;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Plus"))
        {
            Destroy(collision.gameObject);
            GhostManager.Instance.MakeGhostsVulnerable(10f);
        }
    }

    public void Die()
    {
        isAlive = false;
        gameObject.SetActive(false);
        GameManager.Instance.CheckGameOver();
    }
}
