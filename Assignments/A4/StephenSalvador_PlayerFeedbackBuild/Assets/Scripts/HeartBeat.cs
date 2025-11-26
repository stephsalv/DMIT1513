using UnityEngine;

public class HeartbeatTrigger : MonoBehaviour
{
    public AudioSource heartbeatAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            if (heartbeatAudio != null && !heartbeatAudio.isPlaying)
            {
                heartbeatAudio.Play();
                Debug.Log("[Heartbeat] Ghost entered! Starting heartbeat.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            if (heartbeatAudio != null && heartbeatAudio.isPlaying)
            {
                heartbeatAudio.Stop();
                Debug.Log("[Heartbeat] Ghost left! Stopping heartbeat.");
            }
        }
    }
}

