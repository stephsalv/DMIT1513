using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Players & Ghosts")]
    public List<PlayerController> players;
    public GhostManager ghostManager;

    [Header("Game Settings")]
    public int winScore = 30;

    [Header("UI")]
    public GameObject gameOverPanel;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip gameOverClip;

    public bool IsGameOver { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void PlayerDied(PlayerController deadPlayer)
    {
        List<PlayerController> alivePlayers = players.FindAll(p => p.isAlive);

        if (alivePlayers.Count == 1)
        {
            DeclareWinner(alivePlayers[0]);
        }
        else if (alivePlayers.Count == 0)
        {
            EndGameNoWinner();
        }
    }
    public void CheckScoreWinner(PlayerController player)
    {
        if (player.score >= winScore)
        {
            DeclareWinner(player);
        }
    }
    private void DeclareWinner(PlayerController winner)
    {
        // Stop all players
        foreach (var p in players)
            p.isAlive = false;

        // Stop ghosts
        ghostManager?.StopAllGhosts();

        // Show winner UI
        if (winner.playerWonCanvas != null)
            winner.playerWonCanvas.SetActive(true);

        // Show death UI for others
        foreach (var p in players)
        {
            if (p != winner && p.playerDiedCanvas != null)
                p.playerDiedCanvas.SetActive(true);
        }

        GameOver();
    }
    private void EndGameNoWinner()
    {
        foreach (var p in players)
        {
            if (p.playerDiedCanvas != null)
                p.playerDiedCanvas.SetActive(true);
        }

        GameOver();
    }
    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;

        Time.timeScale = 0f;

        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();

        if (sfxSource != null && gameOverClip != null)
            sfxSource.PlayOneShot(gameOverClip);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        StartCoroutine(ReloadSceneAfterDelay(10f));
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }
}
