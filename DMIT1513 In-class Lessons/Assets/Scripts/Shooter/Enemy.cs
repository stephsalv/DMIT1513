using UnityEngine;

public class DropOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab;

    private static bool isQuitting = false;

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded || dropPrefab == null)
            return;

        Instantiate(dropPrefab, transform.position, Quaternion.identity);
    }
}