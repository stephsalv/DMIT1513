using UnityEngine;

public class LightDetector : MonoBehaviour
{
    public Flashlight flashlight;
    public bool isLightOn;

    void Update()
    {
        if (flashlight != null)
        {
            isLightOn = flashlight.IsFlashlightOn();
        }
    }
}

