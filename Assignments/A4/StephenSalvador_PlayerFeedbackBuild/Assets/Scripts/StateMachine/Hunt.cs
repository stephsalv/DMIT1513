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

        // Flat movement toward player
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 dir = (targetPos - transform.position).normalized;

        // Move
        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        // Rotate toward movement
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 5f * Time.deltaTime);
        }

        return this;
    }
}
