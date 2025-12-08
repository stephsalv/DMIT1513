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
            Debug.Log("[Jumpscare] Player entered trigger. Starting jumpscare sequence.");
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

        Debug.Log("[Jumpscare] Finished. Quitting game...");
        Application.Quit();
    }

    // Called by Title Button
    public void TitleButtonJumpScare()
    {
        Debug.Log("[Jumpscare] Title button jumpscare triggered.");
        StartCoroutine(TitleJumpScareSequence());
    }

    private IEnumerator TitleJumpScareSequence()
    {
        if (jumpscareIMG != null)
            jumpscareIMG.SetActive(true);

        if (scream != null)
            scream.Play();

        Debug.Log("[Jumpscare] Playing title jumpscare audio.");

        yield return new WaitForSeconds(1);

        if (scream != null)
            scream.Stop();

        if (jumpscareIMG != null)
            jumpscareIMG.SetActive(false);

        Debug.Log("[Jumpscare] Audio finished. Loading Title scene (Scene 0).");
        SceneManager.LoadScene(0);
    }
}

