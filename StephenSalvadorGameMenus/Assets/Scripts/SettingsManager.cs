using UnityEngine;

public class CarSettingsManager : MonoBehaviour
{
    public static CarSettingsManager Instance { get; private set; }

    public float maxSpeed = 20f;
    public float forwardAccel = 10f;
    public float turnStrength = 50f;

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

