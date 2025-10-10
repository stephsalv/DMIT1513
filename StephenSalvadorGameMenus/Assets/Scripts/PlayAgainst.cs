using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameSelection : MonoBehaviour
{
    public void SelectPlayers()
    {
        SceneManager.LoadScene(3); // Load gameplay scene
    }

    public void SelectBots()
    {
        SceneManager.LoadScene(4); // Load gameplay scene
    }
}