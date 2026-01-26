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

    }
}
