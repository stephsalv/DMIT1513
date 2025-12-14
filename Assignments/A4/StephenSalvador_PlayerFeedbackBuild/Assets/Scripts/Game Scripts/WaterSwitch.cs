using UnityEngine;
using TMPro;
using System.Collections;

public class WaterSprinkler : MonoBehaviour
{
    [Header("Sprinkler Particle System")]
    public ParticleSystem sprinklerParticles;

    [Header("UI")]
    public GameObject uiPanel;
    public TextMeshProUGUI uiText;
    private string promptMessage = "Press [E] to turn off sprinkler";

    [Header("Audio")]
    public AudioSource sprinklerAudio; // assign an AudioSource with the sound
    public float audioDuration = 5f; // duration to play the sound

    private bool playerInRange = false;
    private bool isActive = true;

    public GameObject hunterGhost;

    private void Start()
    {
        if (sprinklerParticles != null)
            sprinklerParticles.Play(); // Start sprinkler active
        else
            Debug.LogWarning("[Sprinkler] Particle system not assigned!");

        if (uiPanel != null)
            uiPanel.SetActive(false); // Hide UI initially
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TurnOffSprinkler();
        }
    }

    private void TurnOffSprinkler()
    {
        if (sprinklerParticles != null && isActive)
        {
            // Stop sprinkler
            sprinklerParticles.Stop();
            isActive = false;
            Debug.Log("[Sprinkler] Sprinkler turned off!");

            // Hide previous UI
            if (uiPanel != null)
                uiPanel.SetActive(false);

            // Play audio for 3 seconds
            if (sprinklerAudio != null)
                StartCoroutine(PlayAudioTemporarily(sprinklerAudio, audioDuration));

            // Activate Hunter ghost
            if (hunterGhost != null)
            {
                hunterGhost.SetActive(true);
                Debug.Log("[Sprinkler] Hunter ghost activated!");

                // Show new UI message for 3 seconds
                if (uiPanel != null && uiText != null)
                {
                    StartCoroutine(ShowTemporaryMessage("Do you hear that? I think something else is out there too...", 3f));
                }
            }
        }
    }

    // Coroutine to show a message temporarily
    private IEnumerator ShowTemporaryMessage(string message, float duration)
    {
        uiText.text = message;
        uiPanel.SetActive(true);

        yield return new WaitForSeconds(duration);

        uiPanel.SetActive(false);
    }

    // Coroutine to play audio temporarily
    private IEnumerator PlayAudioTemporarily(AudioSource audioSource, float duration)
    {
        audioSource.Play();
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        Debug.Log("[Sprinkler] Player in range.");

        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
            if (uiText != null)
                uiText.text = promptMessage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        Debug.Log("[Sprinkler] Player left range.");

        if (uiPanel != null)
            uiPanel.SetActive(false);
    }
}
