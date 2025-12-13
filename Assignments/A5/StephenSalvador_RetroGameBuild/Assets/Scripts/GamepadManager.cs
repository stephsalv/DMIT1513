using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GamepadManager : MonoBehaviour
{
    public List<int> gamepadIDs = new List<int>();

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (!(device is Gamepad)) return;

        switch (change)
        {
            case InputDeviceChange.Added:
                // Add the new gamepad if not already in the list
                if (!gamepadIDs.Contains(device.deviceId))
                {
                    gamepadIDs.Add(device.deviceId);
                    Debug.Log($"Gamepad added: {device.displayName}");
                }
                break;

            case InputDeviceChange.Removed:
                if (gamepadIDs.Contains(device.deviceId))
                {
                    gamepadIDs.Remove(device.deviceId);
                    Debug.Log($"Gamepad removed: {device.displayName}");
                }
                break;
        }
    }

    public int PlayerCount()
    {
        return gamepadIDs.Count;
    }
}
