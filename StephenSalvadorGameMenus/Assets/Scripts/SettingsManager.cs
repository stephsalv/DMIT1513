using UnityEngine;

public class CarSettingsManager : MonoBehaviour
{
    public static CarSettingsManager Instance { get; private set; }

    public float maxSpeed = 50f;
    public float forwardAccel = 30f;
    public float turnStrength = 60f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetMaxSpeed(float value) => maxSpeed = value;
    public void SetForwardAccel(float value) => forwardAccel = value;
    public void SetTurnStrength(float value) => turnStrength = value;
}

