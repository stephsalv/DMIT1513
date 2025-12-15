using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Buttons")]
    public Button twoPlayerButton;
    public Button fourPlayerButton;

    [Header("References")]
    public GamepadManager gamepadManager;

    void Update()
    {
        UpdatePlayerButtons();
    }
    private void UpdatePlayerButtons()
    {
        int playersConnected = gamepadManager.PlayerCount();
        twoPlayerButton.interactable = (playersConnected == 2);
        fourPlayerButton.interactable = (playersConnected == 4);
    }
}
