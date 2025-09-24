using UnityEngine;
using UnityEngine.UI;

public class OrbitAndSpin : MonoBehaviour
{
    public Slider orbitSlider; // Assign in Inspector
    public float orbitRadius = 5f; // Distance from center
    public Transform orbitCenter; // The point to orbit around

    void Update()
    {
        if (orbitSlider == null || orbitCenter == null) return;

        float angle = orbitSlider.value;
        float radians = angle * Mathf.Deg2Rad;

        // Calculate new position on circle
        Vector3 offset = new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians)) * orbitRadius;
        transform.position = orbitCenter.position + offset;

        // Rotate object to face forward or spin on its own axis
        transform.localRotation = Quaternion.Euler(0, angle, 0);
    }
}


