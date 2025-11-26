using UnityEngine;
using TMPro;

public class WaterSprinkler : MonoBehaviour
{
    [Header("Sprinkler Particle System")]
    public ParticleSystem sprinklerParticles;

    [Header("UI")]
    public GameObject uiPanel;
    public TextMeshProUGUI uiText;
    private string promptMessage = "Press [E] to turn off sprinkler";

    private bool playerInRange = false;
    private bool isActive = true;

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
            sprinklerParticles.Stop();
            isActive = false;
            Debug.Log("[Sprinkler] Sprinkler turned off!");

            if (uiPanel != null)
                uiPanel.SetActive(false); // Hide UI when turned off
        }
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

