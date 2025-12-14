using UnityEngine;

public class AttackState : State
{
    public Transform player;
    public float attackRange = 2f;

    public ChaseState chaseState;
    public CryingState cryingState;

    public LightDetector lightDetector;

    [Header("Attack Timing")]
    public float attackDuration = 1.0f;

    private float timer;
    private bool hasAttacked;

    public override void Enter()
    {
        timer = attackDuration;
        hasAttacked = false;
    }

    public override State RunCurrentState()
    {
        // 🔦 SAME LIGHT LOGIC AS FleeLightState
        if (lightDetector != null && lightDetector.isLightOn)
        {
            return cryingState;
        }

        timer -= Time.deltaTime;

        // Damage once during animation
        if (!hasAttacked && timer <= attackDuration * 0.5f)
        {
            hasAttacked = true;
            Debug.Log("Ghost attacks the player!");
            // Apply damage here
        }

        // When attack animation finishes
        if (timer <= 0f)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= attackRange)
                return this;   // chain attack
            else
                return chaseState;
        }

        return this;
    }
}
