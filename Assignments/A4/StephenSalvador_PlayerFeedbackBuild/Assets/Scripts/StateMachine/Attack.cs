using UnityEngine;

public class AttackState : State
{
    public Transform player;
    public float attackRange = 2f;
    public ChaseState chaseState;
    public CryingState cryingState;

    public bool isHitByFlashlight = false; // Set this from your flashlight script
    private float attackCooldown = 1f;
    private float timer = 0f;

    private void Awake()
    {
        timer = attackCooldown; // so it can attack immediately
    }

    public override State RunCurrentState()
    {
        // Transition immediately if hit by flashlight
        if (isHitByFlashlight)
        {
            isHitByFlashlight = false; // reset flag
            return cryingState;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // If player escapes attack range, go back to chasing
        if (distance > attackRange)
        {
            return chaseState;
        }

        // Attack logic
        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            timer = 0f;
            Debug.Log("Ghost attacks the player!");
            // TODO: Apply damage here
        }

        return this;
    }
}

