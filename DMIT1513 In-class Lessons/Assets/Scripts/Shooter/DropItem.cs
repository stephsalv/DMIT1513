using UnityEngine;

public class DropOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab; // The prefab to spawn
    [SerializeField] private Vector3 dropOffset = Vector3.up * 0.5f; // Offset to spawn above object
    [SerializeField] private bool applyRandomRotation = true;

    private void OnDestroy()
    {
        if (dropPrefab != null)
        {
            Quaternion rotation = applyRandomRotation ? Random.rotation : Quaternion.identity;
            Instantiate(dropPrefab, transform.position + dropOffset, rotation);
        }
    }
}
