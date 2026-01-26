using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    int frameIndex = 0;
    GhostData data;
    private void Awake()
    {
        if (GameManager.instance != null && GameManager.instance.currentProfile.ghostData != null)
        {
            Debug.Log("Ghost data loaded");
            data = GameManager.instance.currentProfile.ghostData;
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
            return;
        }

        var tmp = GetComponent<MeshRenderer>().material;

        GetComponent<MeshRenderer>().material = tmp;
    }
    private void FixedUpdate()
    {
        PlayData();
    }
    public void PlayData()
    {
        if (data == null) return;

        if (frameIndex >= data.ghostDataFrames.Count)
            return;
        Debug.Log("Playing ghostdata");

        GhostDataFrame frame = data.ghostDataFrames[frameIndex];

        transform.position = frame.position;
        transform.rotation = Quaternion.Euler(frame.rotation);

        frameIndex++;
    }
}