using UnityEngine;

public class RandomBounce : MonoBehaviour
{
    public float speed = 10f; // Speed of the ball after bounce

    private void OnCollisionEnter(Collision collision)
    {
        // Generate a random direction
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(0.5f, 1f), // Ensure upward movement
            Random.Range(-1f, 1f)
        ).normalized;

        // Apply velocity in the random direction
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = randomDirection * speed;
    }
}
