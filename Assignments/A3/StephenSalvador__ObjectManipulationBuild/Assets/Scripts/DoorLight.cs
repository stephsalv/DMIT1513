using UnityEngine;

public class DoorLight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Renderer doorRenderer; // The object whose color you want to change

    [Header("Colors")]
    [SerializeField] private Color redColor = Color.red;
    [SerializeField] private Color greenColor = Color.green;

    private void Start()
    {
        if (doorRenderer == null)
            doorRenderer = GetComponent<Renderer>();

        UpdateColor();

        // Subscribe to quest updates
        if (QuestManager.Instance != null)
            QuestManager.Instance.OnKeysUpdated += UpdateColor;
    }

    private void OnDestroy()
    {
        // Unsubscribe safely
        if (QuestManager.Instance != null)
            QuestManager.Instance.OnKeysUpdated -= UpdateColor;
    }

    private void UpdateColor()
    {
        if (doorRenderer == null || QuestManager.Instance == null) return;

        // Determine color based on key progress
        bool questComplete = QuestManager.Instance.HasEnoughKeys();
        Color targetColor = questComplete ? greenColor : redColor;

        // Apply color to material
        doorRenderer.material.color = targetColor;

        // Optional: make emission match (if using an emissive material)
        if (doorRenderer.material.HasProperty("_EmissionColor"))
        {
            doorRenderer.material.SetColor("_EmissionColor", targetColor);
        }
    }
}