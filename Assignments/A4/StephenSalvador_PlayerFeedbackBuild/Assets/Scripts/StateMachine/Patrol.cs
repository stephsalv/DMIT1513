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
        Vector3 dir = (target.position - transform.position).normalized;
        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
        }
    }
}

