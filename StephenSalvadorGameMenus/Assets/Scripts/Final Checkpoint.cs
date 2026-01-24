using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalCheckpoint : MonoBehaviour
{
    private bool raceFinished = false;
    public GameObject gameWonPanel;
    public GameObject gameLostPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (raceFinished) return;

        if (other.CompareTag("Player"))
        {
            raceFinished = true;
            Debug.Log("PLAYER WINS!");
            ShowGameWon();
        }
        else if (other.CompareTag("AICar"))
        {
            raceFinished = true;
            Debug.Log("AI WINS!");
            ShowGameLost();
        }
    }

    private void ShowGameWon()
    {
        gameWonPanel.SetActive(true);
    }

    private void ShowGameLost()
    {
        gameLostPanel.SetActive(true);
    }
}
