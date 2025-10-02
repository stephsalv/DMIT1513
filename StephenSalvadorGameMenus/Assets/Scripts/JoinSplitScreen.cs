using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class JoinSplitScreen : MonoBehaviour
{
    public int index = 0;

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

    public void Next()
    {
        index++;
        PlayerPrefs.SetInt("save", index);
        PlayerPrefs.Save();
    }
}

