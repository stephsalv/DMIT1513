using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("Light Settings")]
    public Light targetLight;       // The light to flicker
    public float minIntensity = 10.0f;
    public float maxIntensity = 50.0f;
    public float flickerSpeed = 0.5f; // Time between intensity changes

    [Header("Optional: Range Flicker")]
    public bool flickerRange = false;
    public float minRange = 20f;
    public float maxRange = 40f;

    private float timer;

    void Start()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Flicker();
            timer = flickerSpeed;
        }
    }

    void Flicker()
    {
        if (targetLight != null)
        {
            // Random intensity
            targetLight.intensity = Random.Range(minIntensity, maxIntensity);

            // Optional range flicker
            if (flickerRange)
                targetLight.range = Random.Range(minRange, maxRange);
        }
    }
}