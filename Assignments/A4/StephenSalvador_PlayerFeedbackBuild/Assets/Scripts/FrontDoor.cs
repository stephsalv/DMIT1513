using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FrontDoor : MonoBehaviour
{
    [Header("UI")]
    public GameObject doorUIPanel;
    public TextMeshProUGUI doorMessageText;

    private bool playerInRange = false;

    public GameObject doorModel;

    private void Start()
    {
        if (doorUIPanel != null)
            doorUIPanel.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!QuestManager.Instance.HasTalkedToNPC)
            {
                doorMessageText.text = "You should talk to the trooper first.";
                return;
            }

            doorMessageText.text = "The door opens...";

            if (doorModel != null)
                doorModel.SetActive(false);
            else
                gameObject.SetActive(false); // fallback if no separate model assigned

            // Optionally hide UI after door opens
            if (doorUIPanel != null)
                doorUIPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        doorUIPanel.SetActive(true);

        // Update message based on progress
        if (!QuestManager.Instance.HasTalkedToNPC)
            doorMessageText.text = "The trooper might have something important to say. Press [F]";
        else
            doorMessageText.text = "Press [F] to try the door.";
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        doorUIPanel.SetActive(false);
    }
}
