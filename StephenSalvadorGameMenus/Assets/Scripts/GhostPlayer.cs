using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    public GhostData ghostData;
    int frameIndex;

    void FixedUpdate()
    {
        if (ghostData == null) return;
        if (frameIndex >= ghostData.ghostDataFrames.Count) return;

        GhostDataFrame frame = ghostData.ghostDataFrames[frameIndex];

        transform.position = frame.position;
        transform.rotation = Quaternion.Euler(frame.rotation);

        frameIndex++;
    }
    void Awake()
    {
        if (GameManager.instance.currentProfile.ghostData != null)
        {
            ghostData = GameManager.instance.currentProfile.ghostData;
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
