using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    [Header("Dialogue")]
    public GameObject dialoguePrefab;
    public GameObject dialogueCanvas;
    public Transform dialogueParent;

    private Queue<string> dialogueQueue = new Queue<string>();
    private GameObject currentDialogueBox;
    private bool isInDialogue = false;
    private bool playerDetected = false;
    private bool hasTalked = false;

    [Header("Quest UI")]
    [SerializeField] private QuestUI questUI;

    void Update()
    {
        if (playerDetected && Input.GetKeyDown(KeyCode.F))
        {
            if (!isInDialogue)
            {
                StartDialogue();
            }
            else
            {
                ShowNextDialogue();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerDetected = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerDetected = false;
    }

    private void StartDialogue()
    {
        if (hasTalked) return;

        isInDialogue = true;
        dialogueCanvas.SetActive(true);
        PlayerController.dialogue = true;

        // Dialogue lines
        dialogueQueue.Clear();
        dialogueQueue.Enqueue("Hello, can you please help me?");
        dialogueQueue.Enqueue("We need a key to get to the next level");
        dialogueQueue.Enqueue("Could you defeat the 5 bubbles for me?");
        dialogueQueue.Enqueue("Thank you! Good luck, soldier!");

        ShowNextDialogue();
    }

    private void ShowNextDialogue()
    {
        if (currentDialogueBox != null)
            Destroy(currentDialogueBox);

        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string text = dialogueQueue.Dequeue();
        currentDialogueBox = Instantiate(dialoguePrefab, dialogueParent ? dialogueParent : dialogueCanvas.transform);
        currentDialogueBox.transform.localScale = Vector3.one;
        currentDialogueBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
    }

    private void EndDialogue()
    {
        isInDialogue = false;
        PlayerController.dialogue = false;

        if (currentDialogueBox != null)
            Destroy(currentDialogueBox);

        dialogueCanvas.SetActive(false);

        if (questUI != null && !hasTalked)
        {
            questUI.ShowQuestUI();
            hasTalked = true;
        }
    }
}
