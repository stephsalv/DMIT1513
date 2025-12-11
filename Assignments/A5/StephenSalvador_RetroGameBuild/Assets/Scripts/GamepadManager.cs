using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadManager : MonoBehaviour
{
    [SerializeField] List<int> gamepadID;
    int gamepadsConnected;

    // Start is called before the first frame update
    void Start()
    {
        gamepadsConnected = Gamepad.all.Count;
    }

    // Update is called once per frame
    void Update()
    {
        // If a gamepad got disconnected then find the player object that was connected to it
        if (Gamepad.all.Count < gamepadsConnected)
        {
            gamepadsConnected = Gamepad.all.Count;
            // Go through the list of gamepadIDs
            for (int i = 0; i < gamepadID.Count; i++)
            {
                bool found = false;
                // Search throught the current gamepads that are connected
                for (int j = 0; j < Gamepad.all.Count && !found; j++)
                {
                    // If the gamepadID is in the list of gamepads connected then you found it and do nothing
                    if (gamepadID[i] == Gamepad.all[j].deviceId)
                    {
                        found = true;
                    }
                }
                // If the gamepadID was not found then remove at that index from both lists and destroy the player object
                if (!found)
                {
                    gamepadID[i] = -1;
                }
            }
        }

        if (Gamepad.all.Count > gamepadsConnected)
        {
            gamepadsConnected = Gamepad.all.Count;

            bool found = false;
            for (int i = 0; i < gamepadID.Count && !found; i++)
            {
                if (gamepadID[i] == -1)
                {
                    for (int j = 0; j < Gamepad.all.Count; j++)
                    {
                        if (!gamepadID.Contains(Gamepad.all[j].deviceId))
                        {
                            gamepadID[i] = Gamepad.all[j].deviceId;
                            found = true;
                        }
                    }                    
                }
            }
        }
    }

    public void PlayerJoined(int id)
    {
        if (!gamepadID.Contains(id))
        {
            gamepadID.Add(id);
        }
    }

    public int PlayerCount()
    {
        return gamepadID.Count;
    }

    public int PlayerStatus(int index)
    {
        return gamepadID[index];
    }
}
