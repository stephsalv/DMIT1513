using UnityEngine;

public class IdleState : State
{
    public Transform[] waypoints;
    public float speed = 3f;
    public Transform player;
    public float detectionRange = 10f;

    public ChaseState chaseState;

    private int waypointIndex = 0;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override State RunCurrentState()
    {
        Patrol();

        // Transition to Chase if player detected
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            return chaseState;
        }

        return this;
    }

    private void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[waypointIndex];
        Vector3 dir = (target.position - transform.position).normalized;
        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
        }
    }
}
