using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    public GameObject cameraObject;  // Drag the camera object here
    public bool isOn = true;

    public void TurnOff()
    {
        isOn = false;
        cameraObject.SetActive(false);
    }

    public void TurnOn()
    {
        isOn = true;
        cameraObject.SetActive(true);
    }
}
