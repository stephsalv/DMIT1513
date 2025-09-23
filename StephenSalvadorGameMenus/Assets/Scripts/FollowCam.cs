using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player; // Assign the player object in the Inspector
    public Vector3 offset;   // Offset to maintain distance between camera and player
    public float smoothSpeed = 1f; // Adjust for smoothness

    // void LateUpdate()
    // {
    //     if (player != null)
    //     {
    //         Vector3 desiredPosition = player.position + offset;
    //         Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    //         transform.position = smoothedPosition;
    //     }
    // }
    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = new Vector3(player.position.x + offset.x,
                                                  player.position.y + offset.y,
                                                  player.position.z + offset.z);

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

}
