using UnityEngine;

public class ChaseState : State
{
    public Transform player;
    public float speed = 4f;
    public float attackRange = 2f;
    public float losePlayerRange = 12f;

    public AttackState attackState;
    public IdleState idleState;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override State RunCurrentState()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // Transition to Attack
        if (distance <= attackRange)
            return attackState;

        // Transition to Idle if player too far
        if (distance > losePlayerRange)
            return idleState;

        // Move toward player
        Vector3 dir = (player.position - transform.position).normalized;
        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        return this;
    }
}
