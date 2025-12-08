using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject uiPanel;
    public TextMeshProUGUI uiText;

    [TextArea]
    public string[] dialogueLines;

    private int currentLine = 0;

    void Start()
    {
        uiPanel.SetActive(true);
        ShowLine(currentLine);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }
    }

    void ShowLine(int index)
    {
        if (index < dialogueLines.Length)
            uiText.text = dialogueLines[index];
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            ShowLine(currentLine);
        }
        else
        {
            // End of dialogue, optionally load next scene
            uiPanel.SetActive(false);
            SceneManager.LoadScene(2);
        }
    }
}