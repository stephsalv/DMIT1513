using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton for easy access

    [SerializeField] private int enemiesToUnlock = 5;
    [SerializeField] private GameObject lootPrefab; // Prefab to spawn
    [SerializeField] private Transform lootSpawnPoint; // Where the loot spawns

    private int enemiesDestroyed = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Call this whenever an enemy is destroyed
    public void EnemyDestroyed()
    {
        enemiesDestroyed++;

        if (enemiesDestroyed >= enemiesToUnlock)
        {
            SpawnLoot();
            enemiesDestroyed = 0; // reset counter
        }
    }

    private void SpawnLoot()
    {
        if (lootPrefab != null && lootSpawnPoint != null)
        {
            Instantiate(lootPrefab, lootSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("LootPrefab or SpawnPoint not set in ScoreManager!");
        }
    }
}
