using UnityEngine;

public class Ghost : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;          // Ghost movement speed
    public float directionTime = 2f;      // How long to move in a direction before picking a new one

    private float timer;
    private Vector3 moveDirection;

    private void Start()
    {
        PickNewDirection();
    }

    private void Update()
    {
        // Move ghost
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Countdown
        timer -= Time.deltaTime;

        // Time to change direction?
        if (timer <= 0)
        {
            PickNewDirection();
        }
    }

    void PickNewDirection()
    {
        // Pick random XZ direction (no up/down)
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        moveDirection = new Vector3(x, 0, z).normalized;

        timer = directionTime;

        // Face the direction the ghost is going
        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDirection);
    }
}
