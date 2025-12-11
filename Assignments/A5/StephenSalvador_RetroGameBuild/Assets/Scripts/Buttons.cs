using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    // Called by the Play button
    public void OnPlayPressed()
    {
        SceneManager.LoadScene(1);
    }

    // Called by the Quit button
    public void OnQuitPressed()
    {
        Application.Quit(); // Works in build

        // Works in Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

