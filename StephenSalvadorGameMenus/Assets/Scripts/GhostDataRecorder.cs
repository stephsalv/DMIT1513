using UnityEngine;

public class GhostDataRecorder : MonoBehaviour
{
    public GhostData ghostData = new GhostData();
    public float lapTime { get; private set; }

    bool isRecording;

    private void Start()
    {
        StartRecording();
    }

    public void StartRecording()
    {
        isRecording = true;
    }

    public void StopRecording()
    {
        isRecording = false;
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
