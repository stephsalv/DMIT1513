using UnityEngine;

public class RageState : State
{
    public Transform player;
    public float speed = 8f;
    public float rageDuration = 3f;

    public HuntState huntState;

    private float timer = 0f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override State RunCurrentState()
    {
        timer += Time.deltaTime;

        Vector3 dir = (player.position - transform.position).normalized;
        rb.MovePosition(transform.position + dir * speed * Time.deltaTime);

        if (timer >= rageDuration)
        {
            timer = 0f;
            return huntState;
        }

        return this;
    }
}

