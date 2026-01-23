using UnityEngine;

public class GhostDataRecorder : MonoBehaviour
{
    GhostData ghostData;
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

        Vector3 position = transform.position;
        Vector3 rotation = transform.eulerAngles;
        float time = Time.deltaTime;

        GhostDataFrame frame = new GhostDataFrame
        (
            position,
            rotation,
            Time.timeSinceLevelLoad
        );

        ghostData.ghostDataFrames.Add(frame);
    }
}
