using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    [SerializeField] private int keysCollected = 0;
    [SerializeField] private int keysNeeded = 5;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CollectKey(int amount = 1)
    {
        keysCollected += amount;
        Debug.Log($"Keys collected: {keysCollected}/{keysNeeded}");
    }

    public bool HasEnoughKeys()
    {
        return keysCollected >= keysNeeded;
    }

    public int KeysCollected => keysCollected;
    public int KeysNeeded => keysNeeded;
}
