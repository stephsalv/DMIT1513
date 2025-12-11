using UnityEngine;

public class HuntState : State
{
    public Transform player;
    public float speed = 4f;
    public float attackRange = 2f;

    public FleeLightState fleeState;
    public PatrolState patrolState;
    public LightDetector lightDetector;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override State RunCurrentState()
    {
        // If light turns ON → Flee
        if (lightDetector.isLightOn)
        {
            return fleeState;
        }

        // Move toward player
        Vector3 dir = (player.position - transform.position).normalized;
        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        return this;
    }
}

