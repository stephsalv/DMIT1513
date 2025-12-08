using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Ghost : MonoBehaviour
{
    public int scoreValue = 5;
    public bool isVulnerable = false;

    private AIMover aiMover;
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        aiMover = GetComponent<AIMover>();
    }

    void Update()
    {
        if (!isVulnerable)
        {
            GameObject nearestPlayer = FindClosestPlayer();
            if (nearestPlayer != null)
                aiMover.SetTarget(nearestPlayer);
        }
        else
        {
            FleeFromPlayers();
        }
    }

    private GameObject FindClosestPlayer()
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (var player in GameManager.Instance.players)
        {
            if (!player.isAlive) continue;

            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestPlayer = player.gameObject;
            }
        }

        return closestPlayer;
    }

    private void FleeFromPlayers()
    {
        Vector3 fleeDirection = Vector3.zero;
        int count = 0;

        foreach (var player in GameManager.Instance.players)
        {
            if (!player.isAlive) continue;
            fleeDirection += (transform.position - player.transform.position).normalized;
            count++;
        }

        if (count > 0)
            fleeDirection /= count;

        aiMover.SetTarget(transform.position + fleeDirection * 5f);
    }

    public void Die()
    {
        // Reset state
        isVulnerable = false;
        transform.position = GhostManager.Instance.GetSpawnPosition();
    }
}
