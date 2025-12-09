using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class LobbyW1 : MonoBehaviour
{
    [SerializeField] JoystickManager joystickManager; // changed
    [SerializeField] List<TMP_Text> playerText;
    [SerializeField] Button startButton;

    [SerializeField] GameObject lobbyPanel;

    [SerializeField] PlayerInputManager playerInputManager;

    [SerializeField] GameObject[] spawnLocations;

    // Start is called before the first frame update
    void Start()
    {
        joystickManager = GameObject.Find("JoystickManager").GetComponent<JoystickManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Joystick.current != null)
        {
            // Check if any button was pressed this frame
            bool joinPressed = Joystick.current.allControls
                .OfType<ButtonControl>()
                .Any(b => b.wasPressedThisFrame);

            if (joinPressed)
            {
                joystickManager.PlayerJoined(Joystick.current.deviceId);
            }
        }

        for (int i = 0; i < joystickManager.PlayerCount(); i++)
        {
            if (joystickManager.PlayerStatus(i) > -1)
                playerText[i].text = "Connected";
            else
                playerText[i].text = "Disconnected";
        }

        startButton.interactable = joystickManager.PlayerCount() > 0;
    }

    public void StartGame()
    {
        for (int i = 0; i < joystickManager.PlayerCount(); i++)
        {
            // Join the player with the joystick device
            PlayerInput player = playerInputManager.JoinPlayer(i, -1, null, Joystick.all[i]);
            player.transform.position = spawnLocations[i].transform.position;
        }
        gameObject.SetActive(false);
    }
}
