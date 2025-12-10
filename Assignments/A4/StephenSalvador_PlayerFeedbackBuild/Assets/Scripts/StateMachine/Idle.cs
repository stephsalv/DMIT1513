using UnityEngine;

public class IdleState : State
{
    public Transform[] waypoints;
    public float speed = 3f;
    public Transform player;
    public float detectionRange = 10f;

    public ChaseState chaseState;
    public CryingState cryingState;   // Add reference

    private int waypointIndex = 0;
    private Rigidbody rb;

    private float cryCheckTimer = 0f;
    private float cryCheckInterval = 2f;  // How often to roll random chance
    [Range(0f, 1f)] public float cryChance = 0.25f; // 25% chance

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override State RunCurrentState()
    {
        Patrol();

        // Check for player detection first
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            return chaseState;
        }

        // Random crying trigger (every few seconds, not every frame)
        cryCheckTimer += Time.deltaTime;
        if (cryCheckTimer >= cryCheckInterval)
        {
            cryCheckTimer = 0f;

            if (Random.value < cryChance)
            {
                return cryingState;
            }
        }

        return this;
    }

    private void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[waypointIndex];
        Vector3 dir = (target.position - transform.position).normalized;

        // Move
        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        // Rotate towards movement direction
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 5f * Time.deltaTime);
        }

        // Switch to next waypoint if close enough
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
        }
    }
    private void MoveTowards(Vector3 targetPosition, float moveSpeed)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }
}

