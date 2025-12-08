using UnityEngine;

public class FleeLightState : State
{
    public Transform safePoint;
    public float speed = 5f;

    public PatrolState patrolState;
    public LightDetector lightDetector;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override State RunCurrentState()
    {
        Vector3 dir = (safePoint.position - transform.position).normalized;
        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        // Once lights are off again → start hunting
        if (!lightDetector.isLightOn)
        {
            return patrolState;
        }

        return this;
    }
}
