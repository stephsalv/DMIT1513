using UnityEngine;

public class TopDownCam : MonoBehaviour
{
    [SerializeField] GameObject topView;

    void Update()
    {
        transform.position = topView.transform.position;
    }
}
