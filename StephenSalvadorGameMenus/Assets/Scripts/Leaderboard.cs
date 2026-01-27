using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public SaveSystem saveSystem;

    public TextMeshProUGUI[] placementTexts;

    private void Start()
    {
        RefreshLeaderboard();
    }

    public void RefreshLeaderboard()
    {
        if (saveSystem == null || placementTexts == null || placementTexts.Length == 0)
            return;

        List<SaveProfile> profiles = saveSystem.LoadAllSaveData();
        profiles.RemoveAll(p => p.bestTime <= 0f);
        profiles.Sort((a, b) => a.bestTime.CompareTo(b.bestTime));

        for (int i = 0; i < placementTexts.Length; i++)
        {
            if (i < profiles.Count)
            {
                SaveProfile p = profiles[i];
                placementTexts[i].text = $"{i + 1}. {p.profileName} {p.vehicleName} {p.bestTime:F2}s";
            }
            else
            {
                placementTexts[i].text =
                    $"{i + 1}. ----------";
            }
        }
    }
}
