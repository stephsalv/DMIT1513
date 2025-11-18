using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Door : MonoBehaviour
{
    [Header("UI")]
    public GameObject doorUIPanel; // UI Canvas or Panel for door messages
    public TextMeshProUGUI doorMessageText; // The text showing the message

    private bool playerInRange = false;

    private void Start()
    {
        if (doorUIPanel != null)
            doorUIPanel.SetActive(false);
    }


    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (QuestManager.Instance.HasEnoughKeys())
            {
                int currentScene = SceneManager.GetActiveScene().buildIndex;

                if (currentScene == 0)
                {
                    SceneManager.LoadScene(1); // Go to level 2
                }
                else if (currentScene == 1)
                {
                    SceneManager.LoadScene(2); // Go to level 3
                }
                else if (currentScene == 2)
                {
                    SceneManager.LoadScene(3); // Go to GameOver Scene
                }
                else
                {
                    Debug.Log("No next level! Game completed!");
                }
            }
            else
            {
                int remaining = QuestManager.Instance.KeysNeeded - QuestManager.Instance.KeysCollected;
                doorMessageText.text = $"You need {remaining} more key(s)!";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        doorUIPanel.SetActive(true);

        // Update message based on progress
        if (QuestManager.Instance.HasEnoughKeys())
            doorMessageText.text = "Press [F] to open the door...";
        else
            doorMessageText.text = $"You need {QuestManager.Instance.KeysNeeded - QuestManager.Instance.KeysCollected} more key(s).";
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        doorUIPanel.SetActive(false);
    }
}
