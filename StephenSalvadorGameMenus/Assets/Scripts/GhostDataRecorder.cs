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
    private void FixedUpdate()
    {
        if (!isRecording) return;

        Vector3 position = transform.position;
        Vector3 rotation = transform.eulerAngles;

        GhostDataFrame frame = new GhostDataFrame(position, rotation);

        ghostData.ghostDataFrames.Add(frame);
    }
}
