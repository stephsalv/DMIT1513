using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance;

    [Header("Ghosts")]
    public List<Ghost> ghosts;

    [Header("Spawn")]
    public Transform ghostSpawnPoint; // Assign your spawn point in the Inspector

    private void Awake()
    {
        Instance = this;

        if (ghostSpawnPoint == null)
            Debug.LogWarning("GhostManager: ghostSpawnPoint is not assigned!");
    }

    public void MakeGhostsVulnerable(float duration)
    {
        StartCoroutine(VulnerableRoutine(duration));
    }

    private IEnumerator VulnerableRoutine(float duration)
    {
        foreach (var g in ghosts)
            g.isVulnerable = true;

        yield return new WaitForSeconds(duration);

        foreach (var g in ghosts)
            g.isVulnerable = false;
    }

    public Vector3 GetSpawnPosition()
    {
        if (ghostSpawnPoint != null)
            return ghostSpawnPoint.position;

        // Fallback to random position if spawn point is not assigned
        return new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));
    }

    public void StopAllGhosts()
    {
        foreach (var ghost in ghosts)
            ghost.StopMovement();
    }
}
