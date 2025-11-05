using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Key Progress")]
    [SerializeField] private int keysCollected = 0;
    [SerializeField] private int keysNeeded = 5;

    public event Action OnKeysUpdated;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CollectKey(int amount = 1)
    {
        keysCollected += amount;
        Debug.Log($"Keys collected: {keysCollected}/{keysNeeded}");

        OnKeysUpdated?.Invoke();
    }

    public bool HasEnoughKeys() => keysCollected >= keysNeeded;

    public int KeysCollected => keysCollected;
    public int KeysNeeded => keysNeeded;
}
