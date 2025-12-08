using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [SerializeField] private float panAngle = 30.0f;
    [SerializeField] private float panSpeed = 20.0f;
    private float startYAngle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startYAngle = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Pingpong goes from 1 to (panAngle * 2) and back  
        float pingPong = Mathf.PingPong(Time.time * panSpeed, panAngle * 2);
        // Offset it so it goes from -panAngle to +panAngle
        float targetY = startYAngle - panAngle + pingPong;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, targetY, transform.eulerAngles.z);

    }
}
