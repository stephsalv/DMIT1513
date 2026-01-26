using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CarSettingsUI : MonoBehaviour
{
    public CarStats[] carStatsList;

    public TMP_Text vehicleText;
    public TMP_Text speedText;
    public TMP_Text accelerationText;
    public TMP_Text handlingText;

    public int currentCarIndex = 0;

    void Start()
    {
        UpdateUI(currentCarIndex);
    }
    public void UpdateUI(int index)
    {
        if (index < 0 || index >= carStatsList.Length)
            return;

        currentCarIndex = index;

        CarStats stats = carStatsList[index];

        vehicleText.text = stats.vehicleName;
        speedText.text = stats.speed;
        accelerationText.text = stats.acceleration;
        handlingText.text = stats.handling;
    }
    public void NextCarUI()
    {
        if (currentCarIndex < carStatsList.Length - 1)
        {
            currentCarIndex++;
            UpdateUI(currentCarIndex);
        }
    }
    public void PreviousCarUI()
    {
        if (currentCarIndex > 0)
        {
            currentCarIndex--;
            UpdateUI(currentCarIndex);
        }
    }

    [System.Serializable]
    public class CarStats
    {
        public string vehicleName;
        public string speed;
        public string acceleration;
        public string handling;
    }

}

