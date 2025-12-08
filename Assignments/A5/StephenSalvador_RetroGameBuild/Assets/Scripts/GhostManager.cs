using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance;
    public List<Ghost> ghosts;

    void Awake() { Instance = this; }

    public void MakeGhostsVulnerable(float duration)
    {
        StartCoroutine(VulnerableRoutine(duration));
    }

    private IEnumerator VulnerableRoutine(float duration)
    {
        foreach (var g in ghosts) g.isVulnerable = true;
        yield return new WaitForSeconds(duration);
        foreach (var g in ghosts) g.isVulnerable = false;
    }

    public Vector3 GetSpawnPosition()
    {
        // Random or predefined spawn
        return new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));
    }
}
