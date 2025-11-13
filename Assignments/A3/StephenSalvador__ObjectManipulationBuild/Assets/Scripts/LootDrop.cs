using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [SerializeField] private GameObject keyModel;
    Health health;

    void Start()
    {
        health = GetComponent<Health>();

        if (health != null)
            health.OnDied += DropLoot;
    }

    private void DropLoot()
    {
        Vector3 position = transform.position;
        GameObject key = Instantiate(keyModel, position + Vector3.up, Quaternion.identity);
        key.SetActive(true);
        //Destroy(key, 5f); to increase difficulty
    }
}
