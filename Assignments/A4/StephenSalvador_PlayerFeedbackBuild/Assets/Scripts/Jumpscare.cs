using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jumpscare : MonoBehaviour
{
    [Header("References")]
    public GameObject jumpscareIMG;
    public AudioSource scream;

    [Header("Audio Settings")]
    public float audioDuration = 2f; // Duration to play audio

    void Start()
    {
        // Ensure jumpscare image is hidden at start
        jumpscareIMG.SetActive(false);
    }

    // Triggered when player enters a dangerous area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayerDead());
        }
    }

    // Handles jumpscare for player death
    private IEnumerator PlayerDead()
    {
        // Show image and play scream
        jumpscareIMG.SetActive(true);
        scream.Play();

        // Wait for audioDuration seconds
        yield return new WaitForSeconds(audioDuration);

        // Stop audio and hide image
        scream.Stop();
        jumpscareIMG.SetActive(false);

        // Load Game Over scene
        SceneManager.LoadScene(2);
    }

    // Called by Title Button
    public void TitleButtonJumpScare()
    {
        StartCoroutine(TitleJumpScareSequence());
    }

    private IEnumerator TitleJumpScareSequence()
    {
        // Show image and play scream
        jumpscareIMG.SetActive(true);
        scream.Play();

        // Wait for audioDuration seconds
        yield return new WaitForSeconds(audioDuration);

        // Stop audio and hide image
        scream.Stop();
        jumpscareIMG.SetActive(false);

        // Load Title scene
        SceneManager.LoadScene(0);
    }
}

