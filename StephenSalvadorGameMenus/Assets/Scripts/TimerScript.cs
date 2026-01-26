using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public float time;
    public TextMeshProUGUI timerText;
    private bool running = true;

    void Update()
    {
        if (!running) return;

        time += Time.deltaTime;
        timerText.text = time.ToString("F2");
    }

    public float GetFinalTime()
    {
        return time;
    }

    public void StopTimer()
    {
        running = false;
    }
}

