using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class FollowSplitCamera : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform player;            // The player to follow
    public Transform pos1;              // Camera angle 1
    public Transform pos2;              // Camera angle 2
    public Transform pos3;              // Camera angle 3
    public float moveSpeed = 5f;        // Follow smoothing
    public float lookSpeed = 5f;        // Rotation smoothing
    public int index = 0;               // Active camera angle

    [Header("Split Screen Settings")]
    private Camera cam;
    private int playerIndex;
    private int totalPlayers;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        var input = GetComponentInParent<PlayerInput>();
        if (input != null)
        {
            playerIndex = input.playerIndex;
            cam.depth = playerIndex;
        }

        PlayerInputManager.instance.onPlayerJoined += HandlePlayerJoined;
        totalPlayers = PlayerInput.all.Count;
        SetupCameraViewport();

        // Restore camera angle index if saved
        index = PlayerPrefs.GetInt("save_cam_index", index);
    }

    private void HandlePlayerJoined(PlayerInput input)
    {
        totalPlayers = PlayerInput.all.Count;
        SetupCameraViewport();
    }

    private void SetupCameraViewport()
    {
        switch (totalPlayers)
        {
            case 1:
                cam.rect = new Rect(0, 0, 1, 1);
                break;
            case 2:
                cam.rect = new Rect(playerIndex == 0 ? 0 : 0.5f, 0, 0.5f, 1);
                break;
            case 3:
                if (playerIndex == 0)
                    cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                else if (playerIndex == 1)
                    cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                else
                    cam.rect = new Rect(0, 0, 1, 0.5f);
                break;
            default:
                cam.rect = new Rect((playerIndex % 2) * 0.5f, (playerIndex < 2) ? 0.5f : 0f, 0.5f, 0.5f);
                break;
        }
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

    public void AssignPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }
}
