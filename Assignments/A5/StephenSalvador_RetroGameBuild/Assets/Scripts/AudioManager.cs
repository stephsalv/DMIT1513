using UnityEngine;
using System.Collections.Generic;

public class WalkingAudioManager : MonoBehaviour
{
    public static WalkingAudioManager Instance;

    public AudioSource audioSource;
    public AudioClip walkClip;

    private bool isPlaying = false;

    private void Awake()
    {
        Instance = this;
        audioSource.loop = true;
        audioSource.clip = walkClip;
    }

    void Update()
    {
        if (AnyPlayerMoving() && !isPlaying)
        {
            audioSource.Play();
            isPlaying = true;
        }
        else if (!AnyPlayerMoving() && isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }

    private bool AnyPlayerMoving()
    {
        foreach (var player in GameManager.Instance.players)
        {
            if (player != null && player.isAlive)
            {
                if (player.rb.linearVelocity.magnitude > 0.1f)
                    return true;
            }
        }
        return false;
    }
}
