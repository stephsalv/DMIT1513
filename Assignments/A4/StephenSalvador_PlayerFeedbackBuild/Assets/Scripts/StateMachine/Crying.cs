using UnityEngine;

public class CryingState : State
{
    public IdleState idleState;
    public float cryingDuration = 3f;

    private float timer = 0f;
    private Rigidbody rb;
    private AudioSource audioSource;
    private bool hasPlayedAudio = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public override State RunCurrentState()
    {
        // Stop movement
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }

        // Play crying audio once
        if (!hasPlayedAudio)
        {
            audioSource.Play();
            hasPlayedAudio = true;
        }

        // Count timer
        timer += Time.deltaTime;

        if (timer >= cryingDuration)
        {
            timer = 0f;
            audioSource.Stop();
            hasPlayedAudio = false;
            return idleState;
        }

        return this;
    }
}

