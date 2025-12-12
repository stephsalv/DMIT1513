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

    public int winScore = 25;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // -------------------------------------------------
    // CALLED BY PLAYER CONTROLLER WHEN SOMEONE DIES
    // -------------------------------------------------
    public void PlayerDied(PlayerController deadPlayer)
    {
        // Check how many alive players remain
        List<PlayerController> alivePlayers = players.FindAll(p => p.isAlive);

        // If only one left → they win
        if (alivePlayers.Count == 1)
        {
            DeclareWinner(alivePlayers[0]);
        }
        else if (alivePlayers.Count == 0)
        {
            // (Extremely rare) If all die at same time
            EndGameNoWinner();
        }
    }

    // -------------------------------------------------
    // CALLED BY PLAYER CONTROLLER WHEN FRUIT IS PICKED UP
    // -------------------------------------------------
    public void CheckScoreWinner(PlayerController player)
    {
        if (player.score >= winScore)
        {
            DeclareWinner(player);
        }
    }

    // -------------------------------------------------
    // DECLARE WINNER
    // -------------------------------------------------
    private void DeclareWinner(PlayerController winner)
    {
        // Stop all players
        foreach (var p in players)
            p.isAlive = false;

        // Stop ghosts
        if (ghostManager != null)
            ghostManager.StopAllGhosts();

        // Show win canvas for winner
        if (winner.playerWonCanvas != null)
            winner.playerWonCanvas.SetActive(true);

        // Show death canvas for others
        foreach (var p in players)
        {
            if (p != winner)
            {
                if (p.playerDiedCanvas != null)
                    p.playerDiedCanvas.SetActive(true);
            }
        }

        // Start scene reload
        GameOver();
    }

    // If all players died simultaneously
    private void EndGameNoWinner()
    {
        foreach (var p in players)
        {
            if (p.playerDiedCanvas != null)
                p.playerDiedCanvas.SetActive(true);
        }

        GameOver();
    }

    // -------------------------------------------------
    // RELOAD SCENE
    // -------------------------------------------------
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
