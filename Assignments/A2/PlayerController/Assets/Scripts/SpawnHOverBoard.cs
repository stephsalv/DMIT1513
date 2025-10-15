using UnityEngine;

public class SpawnAndDestroy : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform spawnPoint;

    private GameObject currentObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Spawn object
            currentObject = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }

        if (Input.GetKeyDown(KeyCode.X) && currentObject != null)
        {
            // Destroy object
            Destroy(currentObject);
        }
    }
}
