using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Buttons : MonoBehaviour
{
    public void Play()
    {
        Time.timeScale = 1f; // Reset time before switching scenes
        SceneManager.LoadScene(1);
    }
    public void ReturnToTitle()
    {
        Time.timeScale = 1f; // Reset time before switching scenes
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}