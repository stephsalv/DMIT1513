using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject[] gameObjects;
    public TextMeshProUGUI[] playerName;
    public TextMeshProUGUI[] raceTime;
    public TextMeshProUGUI[] vehicleType;

    private void Start()
    {
        ShowLeaderboard();
    }

    private void ShowLeaderboard()
    {
        if (SaveSystem.instance == null || SaveSystem.instance.saveData == null)
        {
            Debug.LogWarning("SaveSystem not found or no data available.");
            return;
        }

        var profiles = SaveSystem.instance.saveData.profiles;

        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (i < profiles.Count)
            {
                gameObjects[i].SetActive(true);

                playerName[i].text = profiles[i].profileName;
                raceTime[i].text = profiles[i].bestTime.ToString("F2") + "s";
                vehicleType[i].text = profiles[i].vehicle;
            }
            else
            {
                gameObjects[i].SetActive(false);
            }
        }
    }
}
