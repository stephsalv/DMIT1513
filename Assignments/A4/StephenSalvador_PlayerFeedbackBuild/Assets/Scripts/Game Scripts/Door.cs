using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Door : MonoBehaviour
{
    [Header("UI")]
    public GameObject doorUIPanel;
    public TextMeshProUGUI doorMessageText;

    [Header("Door")]
    public GameObject doorModel;

    private bool playerInRange = false;

    private void Start()
    {
        if (doorUIPanel != null)
            doorUIPanel.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(OpenAndCloseDoor());
        }
    }

    private IEnumerator OpenAndCloseDoor()
    {
        if (doorMessageText != null)
            doorMessageText.text = "The door opens...";

        // Open door
        if (doorModel != null)
            doorModel.SetActive(false);

        // Hide UI while door is operating
        if (doorUIPanel != null)
            doorUIPanel.SetActive(false);

        // Wait 2 seconds
        yield return new WaitForSeconds(2f);

        // Close door
        if (doorModel != null)
            doorModel.SetActive(true);

        if (doorMessageText != null)
            doorMessageText.text = "Press [E] to try the door.";

        // Show UI again only if player is still in range
        if (playerInRange && doorUIPanel != null)
            doorUIPanel.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (doorUIPanel != null)
            doorUIPanel.SetActive(true);

        if (doorMessageText != null)
            doorMessageText.text = "Press [E] to try the door.";
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (doorUIPanel != null)
            doorUIPanel.SetActive(false);
    }
}
