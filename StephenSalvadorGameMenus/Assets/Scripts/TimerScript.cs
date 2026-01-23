using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public float time;
    public TextMeshProUGUI timerText;
    void Update()
    {
        time += Time.deltaTime;

        string formatted = time.ToString("F2");
        timerText.text = formatted;
    }
}
