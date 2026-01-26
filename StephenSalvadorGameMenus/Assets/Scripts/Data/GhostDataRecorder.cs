using UnityEngine;

public class GhostDataRecorder : MonoBehaviour
{
    GhostData ghostData;
    bool isRecording;
    public float lapTime { get; private set; }

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
}
