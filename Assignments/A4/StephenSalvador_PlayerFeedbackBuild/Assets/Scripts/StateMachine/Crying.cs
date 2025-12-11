using UnityEngine;

public class CryingState : State
{
    public IdleState idleState;
    public float cryingDuration = 7f;

    private float timer = 0f;
    private Rigidbody rb;
    public AudioSource audioSource;
    private bool hasPlayedAudio = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override State RunCurrentState()
    {
        // Stop movement completely while crying
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Play audio once when entering this state
        if (!hasPlayedAudio && audioSource != null)
        {
            audioSource.Play();
            hasPlayedAudio = true;
        }

        // Countdown crying duration
        timer += Time.deltaTime;

        if (timer >= cryingDuration)
        {
            timer = 0f;

            // Stop audio and reset flag
            if (audioSource != null)
                audioSource.Stop();
            hasPlayedAudio = false;

            // Return to IdleState
            return idleState;
        }

        return this;
    }
}
