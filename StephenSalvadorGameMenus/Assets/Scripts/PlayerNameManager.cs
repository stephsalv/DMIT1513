using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerNameManager : MonoBehaviour
{
    public TMP_InputField nameInputTMP;
    public Button submitButton;
    public TMP_Text nameDisplayTMP;

    private const string PlayerNameKey = "PlayerName";

    void Start()
    {
        submitButton.onClick.AddListener(StoreAndDisplayName);

        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            string savedName = PlayerPrefs.GetString(PlayerNameKey);
            nameDisplayTMP.text = $"Welcome, {savedName}!";
        }
    }

    void StoreAndDisplayName()
    {
        string playerName = nameInputTMP.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString(PlayerNameKey, playerName);
            nameDisplayTMP.text = $"Welcome, {playerName}!";
        }
    }
    private void Awake()
    {
        PlayerPrefs.DeleteKey("PlayerName");
    }

}


