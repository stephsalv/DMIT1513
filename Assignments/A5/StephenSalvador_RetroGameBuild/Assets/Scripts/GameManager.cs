using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<PlayerController> players;
    public GhostManager ghostManager;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void PlayerDied(PlayerController deadPlayer)
    {
        // Stop all player movement
        foreach (var p in players)
        {
            p.isAlive = false;
        }

        // Stop ghosts
        if (ghostManager != null)
            ghostManager.StopAllGhosts();

        // Show winner canvas for surviving player(s)
        foreach (var p in players)
        {
            if (p != deadPlayer)
            {
                if (p.playerWonCanvas != null)
                    p.playerWonCanvas.SetActive(true);
            }
        }
    }
    public void GameOver()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
}
