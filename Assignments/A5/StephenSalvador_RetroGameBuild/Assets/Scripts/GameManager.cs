using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<PlayerController> players;

    void Awake() { Instance = this; }

    public void CheckGameOver()
    {
        // Last alive
        int alive = 0;
        PlayerController winner = null;
        foreach (var p in players)
        {
            if (p.isAlive)
            {
                alive++;
                winner = p;
            }
            // Check score
            if (p.score >= 100)
            {
                Debug.Log(p.name + " wins by points!");
                EndGame();
                return;
            }
        }

        if (alive <= 1 && winner != null)
        {
            Debug.Log(winner.name + " wins!");
            EndGame();
        }
    }

    void EndGame()
    {
        // Stop all movement
        foreach (var p in players) p.enabled = false;
        // Show UI or restart
    }
}
