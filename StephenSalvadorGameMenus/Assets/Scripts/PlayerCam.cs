using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform player;            // The player to follow
    public Transform pos1;              // Camera angle 1
    public Transform pos2;              // Camera angle 2
    public Transform pos3;              // Camera angle 3
    public float moveSpeed = 5f;        // Follow smoothing
    public float lookSpeed = 5f;        // Rotation smoothing
    public int index = 0;               // Active camera angle
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        // Restore camera angle index if saved
        index = PlayerPrefs.GetInt("save_cam_index", index);
    }


    private void LateUpdate()
    {
        if (player == null) return;

        // Choose which position transform to use
        Transform desiredPosTransform = index switch
        {
            0 => pos1,
            1 => pos2,
            2 => pos3,
            _ => pos1
        };

        // Desired world position relative to player
        Vector3 desiredPos = player.TransformPoint(desiredPosTransform.localPosition);
        Quaternion desiredRot = Quaternion.LookRotation(player.position - desiredPosTransform.position, Vector3.up);

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPos, moveSpeed * Time.deltaTime);

        // Smooth look
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, lookSpeed * Time.deltaTime);
    }

    public void NextCameraAngle()
    {
        index++;
        if (index > 2) index = 0;

        PlayerPrefs.SetInt("save_cam_index", index);
        PlayerPrefs.Save();
    }
}
