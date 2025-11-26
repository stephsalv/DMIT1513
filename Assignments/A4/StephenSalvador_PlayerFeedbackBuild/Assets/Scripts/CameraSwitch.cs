using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public SecurityCamera cameraToFix;
    private bool playerInRange = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            cameraToFix.TurnOn();
            Debug.Log("Camera fixed!");
        }
    }
}

