using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject questPanel; // Panel or Canvas group
    [SerializeField] private TextMeshProUGUI keyText;

    private void Start()
    {
        if (questPanel != null)
            questPanel.SetActive(false); // Hide UI at start
    }

    public void ShowQuestUI()
    {
        Debug.Log("Quest UI ShowQuestUI called!");
        if (questPanel != null)
        { 
            questPanel.SetActive(true);
        }
        UpdateKeyText();
    }

    public void UpdateKeyText()
    {
        if (keyText == null || QuestManager.Instance == null) return;

        keyText.text = $"Keys: {QuestManager.Instance.KeysCollected} / {QuestManager.Instance.KeysNeeded}";
    }
}