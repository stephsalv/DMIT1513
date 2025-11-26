using TMPro;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [Header("Camera to Fix")]
    public SecurityCamera cameraToFix;

    [Header("UI")]
    public GameObject uiPanel;
    public TextMeshProUGUI uiText;
    private string promptMessage = "Press [E] to fix camera";

    private bool playerInRange = false;

    private void Start()
    {
        if (uiPanel != null)
            uiPanel.SetActive(false);
        else
            Debug.LogWarning("[CameraSwitch] UI Panel not assigned!");

        if (uiText == null)
            Debug.LogWarning("[CameraSwitch] TMP Text not assigned!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        Debug.Log("[CameraSwitch] Player entered camera trigger!");

        if (uiPanel != null)
            uiPanel.SetActive(true);

        if (uiText != null)
            uiText.text = promptMessage;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        Debug.Log("[CameraSwitch] Player exited camera trigger!");

        if (uiPanel != null)
            uiPanel.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (cameraToFix != null)
            {
                cameraToFix.TurnOn();
                Debug.Log("[CameraSwitch] Player pressed E. Camera fixed!");

                if (uiPanel != null)
                    uiPanel.SetActive(false);
            }
            else
            {
                Debug.LogWarning("[CameraSwitch] CameraToFix not assigned!");
            }
        }
    }
}
