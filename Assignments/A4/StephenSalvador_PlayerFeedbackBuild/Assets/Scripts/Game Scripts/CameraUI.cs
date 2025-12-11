using UnityEngine;
using TMPro;

public class CameraUIManager : MonoBehaviour
{
    public static CameraUIManager Instance; // Singleton for easy access
    public TextMeshProUGUI alertText;
    public float alertDuration = 3f;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowAlert(string message)
    {
        StopAllCoroutines();
        StartCoroutine(AlertRoutine(message));
    }

    private System.Collections.IEnumerator AlertRoutine(string msg)
    {
        alertText.text = msg;
        alertText.enabled = true;

        yield return new WaitForSeconds(alertDuration);

        alertText.enabled = false;
        alertText.text = "";
    }
}