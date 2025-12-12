using NUnit.Framework.Interfaces;
using UnityEngine;

public class PatrolState : State
{
    public Transform[] waypoints;
    public float speed = 2f;
    public Transform player;
    public float detectionRange = 8f;

    public HuntState huntState;
    public RageState rageState;
    public LightDetector lightDetector;

    private Rigidbody rb;
    private int waypointIndex = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override State RunCurrentState()
    {
        Patrol();

        // If lights go OFF → Rage
        if (!lightDetector.isLightOn)
        {
            return rageState;
        }

        return this;
    }

    private void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[waypointIndex];

        // Keep movement flat
        Vector3 flatTargetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 dir = (flatTargetPos - transform.position).normalized;

        // Move
        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        // Rotate toward movement direction
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 5f * Time.deltaTime);
        }

        // Advance waypoint
        if (Vector3.Distance(transform.position, flatTargetPos) < 0.5f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
        }
    }

}

