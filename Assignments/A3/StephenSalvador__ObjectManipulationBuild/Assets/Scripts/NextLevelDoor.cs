using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (QuestManager.Instance.HasEnoughKeys())
        {
            Debug.Log("You have enough keys! Loading next scene...");
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("You need more keys to open this door!");
        }
    }
}
