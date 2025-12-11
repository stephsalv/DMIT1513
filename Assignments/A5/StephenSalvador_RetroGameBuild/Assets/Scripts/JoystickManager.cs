using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickManager : MonoBehaviour
{
    [SerializeField] List<int> joystickID;
    int joysticksConnected;

    // Start is called before the first frame update
    void Start()
    {
        joysticksConnected = Joystick.all.Count;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle disconnected joysticks
        if (Joystick.all.Count < joysticksConnected)
        {
            joysticksConnected = Joystick.all.Count;

            for (int i = 0; i < joystickID.Count; i++)
            {
                bool found = false;

                for (int j = 0; j < Joystick.all.Count && !found; j++)
                {
                    if (joystickID[i] == Joystick.all[j].deviceId)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    joystickID[i] = -1;
                }
            }
        }

        // Handle newly connected joysticks
        if (Joystick.all.Count > joysticksConnected)
        {
            joysticksConnected = Joystick.all.Count;

            bool found = false;
            for (int i = 0; i < joystickID.Count && !found; i++)
            {
                if (joystickID[i] == -1)
                {
                    for (int j = 0; j < Joystick.all.Count; j++)
                    {
                        if (!joystickID.Contains(Joystick.all[j].deviceId))
                        {
                            joystickID[i] = Joystick.all[j].deviceId;
                            found = true;
                        }
                    }
                }
            }
        }
    }

    public void PlayerJoined(int id)
    {
        if (!joystickID.Contains(id))
        {
            joystickID.Add(id);
        }
    }

    public int PlayerCount()
    {
        return joystickID.Count;
    }

    public int PlayerStatus(int index)
    {
        return joystickID[index];
    }
}
