using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GamepadManager : MonoBehaviour
{
    public List<int> controllerIDs = new List<int>();

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        RegisterExistingDevices();
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void RegisterExistingDevices()
    {
        foreach (var device in InputSystem.devices)
        {
            TryAddController(device);
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (!IsValidController(device))
            return;

        switch (change)
        {
            case InputDeviceChange.Added:
                TryAddController(device);
                break;

            case InputDeviceChange.Removed:
                RemoveController(device);
                break;
        }
    }

    /// <summary>
    /// ONLY allow Gamepad or Joystick.
    /// Explicitly rejects Mouse and Keyboard.
    /// </summary>
    private bool IsValidController(InputDevice device)
    {
        // HARD BLOCK desktop inputs
        if (device is Keyboard || device is Mouse)
            return false;

        // ONLY allow actual controllers
        return device is Gamepad || device is Joystick;
    }

    private void TryAddController(InputDevice device)
    {
        if (controllerIDs.Contains(device.deviceId))
            return;

        controllerIDs.Add(device.deviceId);
        Debug.Log($"Controller added: {device.displayName} ({device.GetType().Name})");
    }

    private void RemoveController(InputDevice device)
    {
        if (controllerIDs.Remove(device.deviceId))
        {
            Debug.Log($"Controller removed: {device.displayName}");
        }
    }

    public int PlayerCount()
    {
        return controllerIDs.Count;
    }
}
