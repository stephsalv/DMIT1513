using UnityEngine;

public class GhostDataRecorder : MonoBehaviour
{
    public GhostData ghostData { get; private set; }
    public float lapTime { get; private set; }

    bool isRecording;

    private void Awake()
    {
        ghostData = new GhostData();
    }

    public void StartRecording()
    {
        ghostData = new GhostData();
        lapTime = 0f;
        isRecording = true;
    }

    public void StopRecording()
    {
        isRecording = false;
        ghostData.bestTime = lapTime;
    }

    private void FixedUpdate()
    {
        if (!isRecording) return;

        ghostData.AddFrame(transform.position, transform.eulerAngles);
        lapTime += Time.fixedDeltaTime;
    }

    public GhostData GetGhostData()
    {
        return ghostData;
    }
}
