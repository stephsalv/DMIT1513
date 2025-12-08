using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int score = 0;
    public bool isAlive = true;

    private Rigidbody rb;
    private Vector2 _moveDirection;

    public InputActionReference move;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        // Convert 2D input to 3D movement (x, z)
        Vector3 movement = new Vector3(_moveDirection.x, 0f, _moveDirection.y) * moveSpeed;
        rb.linearVelocity = movement;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isAlive) return;

        // Player vs Player
        if (collision.gameObject.CompareTag("Player"))
        {
            Die();
            collision.gameObject.GetComponent<PlayerController>()?.Die();
        }

        // Player vs Ghost
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

        // Player vs Fruit
        if (collision.gameObject.CompareTag("Fruit"))
        {
            score += 1;
            Destroy(collision.gameObject);
        }

        // Player vs Plus Powerup
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
