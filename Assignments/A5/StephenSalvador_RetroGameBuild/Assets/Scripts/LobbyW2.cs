using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LobbyW2 : MonoBehaviour
{
    [SerializeField] GamepadManager gamepadManager;
    [SerializeField] List<TMP_Text> playerText;
    [SerializeField] Button startButton;

    [SerializeField] GameObject lobbyPanel;

    [SerializeField] PlayerInputManager playerInputManager;

    [SerializeField] GameObject[] spawnLocations;

    // Start is called before the first frame update
    void Start()
    {
        gamepadManager = GameObject.Find("GamepadManager").GetComponent<GamepadManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                gamepadManager.PlayerJoined(Gamepad.current.deviceId);
            }
        }

        for (int i = 0; i < gamepadManager.PlayerCount(); i++)
        {
            if (gamepadManager.PlayerStatus(i) > -1)
            {
                playerText[i].text = "Connected";
            }
            if (gamepadManager.PlayerStatus(i) == -1)
            {
                playerText[i].text = "Disconnected";
            }
        }

        if (gamepadManager.PlayerCount() == 0)
        {
            startButton.interactable = false;
        }
        else
        {
            startButton.interactable = true;
        }
    }

    public void StartGame()
    {
        for (int i = 0; i < gamepadManager.PlayerCount(); i++)
        {
            PlayerInput player = playerInputManager.JoinPlayer(i, -1, null, Gamepad.all[i]);
            player.transform.position = spawnLocations[i].transform.position;
        }
        gameObject.SetActive(false);
    }
}
