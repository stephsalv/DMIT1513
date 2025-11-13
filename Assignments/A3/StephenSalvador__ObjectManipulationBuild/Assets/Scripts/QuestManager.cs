using UnityEngine;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Key Progress")]
    [SerializeField] private int keysCollected = 0;
    [SerializeField] private int keysNeeded = 5;

    public event Action OnKeysUpdated;

    [Header("Quest Progress")]
    public bool HasTalkedToNPC { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CollectKey(int amount = 1)
    {
        keysCollected += amount;
        OnKeysUpdated?.Invoke();
    }

    public bool HasEnoughKeys() => keysCollected >= keysNeeded;

    public void SetTalkedToNPC() => HasTalkedToNPC = true;

    public int KeysCollected => keysCollected;
    public int KeysNeeded => keysNeeded;
}
