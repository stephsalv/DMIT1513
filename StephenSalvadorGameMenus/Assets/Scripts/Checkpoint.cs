using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class Checkpoint : MonoBehaviour
{
    public int index;
    public Checkpoint[] checkpoints;
    public FinalCheckpoint finalCheckpoint;

    public float speedBoostMultiplier = 1.1f;
    public float speedBoostDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"Player hit checkpoint {index}");

        gameObject.SetActive(false);

        CarController car = other.GetComponent<CarController>();
        if (car != null)
        {
            car.ApplySpeedBoost(speedBoostMultiplier, speedBoostDuration);
        }

        if (index == checkpoints.Length - 1)
        {
            finalCheckpoint.gameObject.SetActive(true);
        }
        else
        {
            checkpoints[index + 1].gameObject.SetActive(true);
        }
    }
}


