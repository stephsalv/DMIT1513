using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    public GameObject d_template; // The dialogue prefab
    public GameObject canva;      // The dialogue canvas
    public Transform dialogueParent; // Optional: parent for dialogue UI

    private bool playerDetected = false;
    private bool isInDialogue = false;
    private Queue<string> dialogueQueue = new Queue<string>();
    private GameObject currentDialogueBox;

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

    void StartDialogue()
    {
        isInDialogue = true;
        canva.SetActive(true);
        PlayerController.dialogue = true;

        // Add all dialogue lines to the queue
        dialogueQueue.Clear();
        dialogueQueue.Enqueue("Hello, can you please help me?");
        dialogueQueue.Enqueue("We need a key to get to the next level");
        dialogueQueue.Enqueue("Could you defeat the 5 bubbles for me?");
        dialogueQueue.Enqueue("Thank you! Goodluck, soldier!");

        ShowNextDialogue();
    }

    void ShowNextDialogue()
    {
        // Remove the previous dialogue box
        if (currentDialogueBox != null)
            Destroy(currentDialogueBox);

        // If no more lines, end dialogue
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Get the next line and instantiate the dialogue UI
        string text = dialogueQueue.Dequeue();
        currentDialogueBox = Instantiate(d_template, dialogueParent ? dialogueParent : canva.transform);
        currentDialogueBox.transform.localScale = Vector3.one;
        currentDialogueBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
    }

    void EndDialogue()
    {
        isInDialogue = false;
        PlayerController.dialogue = false;
        canva.SetActive(false);

        if (currentDialogueBox != null)
            Destroy(currentDialogueBox);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerDetected = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerDetected = false;
    }
}
