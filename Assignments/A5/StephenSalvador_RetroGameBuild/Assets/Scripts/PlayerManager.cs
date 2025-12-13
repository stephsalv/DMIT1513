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

        // Enable 2-player only if exactly 2 players connected
        twoPlayerButton.interactable = (playersConnected == 2);

        // Enable 4-player only if exactly 4 players connected
        fourPlayerButton.interactable = (playersConnected == 4);
    }
}
