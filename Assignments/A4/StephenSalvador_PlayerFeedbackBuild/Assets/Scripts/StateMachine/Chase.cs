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
        MoveTowards(player.position, speed);

        return this;
    }
    private void MoveTowards(Vector3 targetPosition, float moveSpeed)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float rotationSpeed = 10f; // increase rotation speed
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
