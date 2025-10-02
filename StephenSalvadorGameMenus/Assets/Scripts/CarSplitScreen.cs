using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class FollowSplitCamera : MonoBehaviour
{
    [Header("Follow Settings")]
    public GameObject player, target;
    public Transform pos1, pos2, pos3;
    public float speed = 1.5f;
    public int index = 0;

    [Header("Split Screen Settings")]
    private Camera cam;
    private int totalPlayers;

    private void Awake()
    {
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
        index = GetComponentInParent<PlayerInput>().playerIndex;
        cam.depth = index;

        PlayerInputManager.instance.onPlayerJoined += HandlePlayerJoined;

        player = GameObject.FindGameObjectWithTag("Player");
        target = GameObject.FindGameObjectWithTag("Target");

        pos1 = GameObject.FindGameObjectWithTag("Pos1").transform;
        pos2 = GameObject.FindGameObjectWithTag("Pos2").transform;
        pos3 = GameObject.FindGameObjectWithTag("Pos3").transform;

        index = PlayerPrefs.GetInt("save", index); // fallback to PlayerInput index if no save
        totalPlayers = PlayerInput.all.Count;

        SetupCamera();
    }

    private void HandlePlayerJoined(PlayerInput obj)
    {
        totalPlayers = PlayerInput.all.Count;
        SetupCamera();
    }

    private void SetupCamera()
    {
        if (totalPlayers == 1)
        {
            cam.rect = new Rect(0, 0, 1, 1);
        }
        else if (totalPlayers == 2)
        {
            cam.rect = new Rect(index == 0 ? 0 : 0.5f, 0, 0.5f, 1);
        }
        else if (totalPlayers == 3)
        {
            cam.rect = new Rect(
                index == 0 ? 0 : (index == 1 ? 0.5f : 0),
                index < 2 ? 0.5f : 0,
                index < 2 ? 0.5f : 1,
                0.5f);
        }
        else
        {
            cam.rect = new Rect((index % 2) * 0.5f, (index < 2) ? 0.5f : 0f, 0.5f, 0.5f);
        }
    }

    private void Update()
    {
        if (index > 2) index = 0;
        if (index < 0) index = 2;
    }

    private void FixedUpdate()
    {
        if (player == null || target == null) return;

        transform.LookAt(player.transform);
        float carMove = Mathf.Abs(Vector3.Distance(transform.position, target.transform.position) * speed);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, carMove * Time.deltaTime);

        if (index == 0)
        {
            target.transform.position = pos1.position;
        }
        if (index == 1)
        {
            target.transform.position = pos2.position;
        }
        if (index == 2)
        {
            target.transform.position = pos3.position;
        }
    }

    public void Next()
    {
        index++;
        PlayerPrefs.SetInt("save", index);
        PlayerPrefs.Save();
    }
}

