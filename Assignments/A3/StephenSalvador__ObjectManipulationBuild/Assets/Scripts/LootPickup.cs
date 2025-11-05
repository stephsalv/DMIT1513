using UnityEngine;

public class LootPickup : MonoBehaviour
{
    [SerializeField] private int keyValue = 1; // Number of keys this pickup counts as

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger entered by: " + other.name);
            QuestManager.Instance.CollectKey(keyValue);
            Destroy(gameObject); // Remove the key from the scene
        }
    }
}
