using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; } = false;

    public static void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f; // Optional: pause physics/animations
    }

    public static void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }

    public static void TogglePause()
    {
        if (IsPaused) Resume();
        else Pause();
    }
}
