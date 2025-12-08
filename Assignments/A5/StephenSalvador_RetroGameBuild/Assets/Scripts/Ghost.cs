using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using NUnit.Framework;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public int scoreValue = 5;
    public bool isVulnerable = false;

    public float fleeDistance = 5f;      // How far to flee when vulnerable
    public float updateRate = 0.2f;      // How often to update target

    private NavMeshAgent agent;
    public GameObject currentTarget;
    public List<GameObject> trackedPlayers = new List<GameObject>();
    private float timer = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            if (!isVulnerable)
            {
                UpdateCurrentTarget();  // always pick closest
                ChaseCurrentTarget();
            }
            else
            {
                FleeFromPlayers();
            }
            timer = 0f;
        }
    }

    // Update the current target to the closest player in the tracked list
    private void UpdateCurrentTarget()
    {
        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (var player in trackedPlayers)
        {
            if (player == null) continue;
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc == null || !pc.isAlive) continue;

            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestPlayer = player;
            }
        }

        currentTarget = closestPlayer;
    }

    private void ChaseCurrentTarget()
    {
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.transform.position);
        }
    }

    private void FleeFromPlayers()
    {
        Vector3 fleeDirection = Vector3.zero;
        int count = 0;

        foreach (var player in trackedPlayers)
        {
            if (player == null) continue;
            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc == null || !pc.isAlive) continue;

            fleeDirection += (transform.position - player.transform.position).normalized;
            count++;
        }

        if (count == 0) return;

        fleeDirection /= count;

        Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

        if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerController hitPlayer = collision.gameObject.GetComponent<PlayerController>();
        if (hitPlayer == null || !hitPlayer.isAlive) return;

        if (isVulnerable)
        {
            hitPlayer.score += scoreValue;
            Die();
        }
        else
        {
            hitPlayer.Die();
        }
    }

    public void Die()
    {
        isVulnerable = false;
        transform.position = GhostManager.Instance.GetSpawnPosition();
    }

    // Trigger system to add/remove players
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !trackedPlayers.Contains(other.gameObject))
        {
            trackedPlayers.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && trackedPlayers.Contains(other.gameObject))
        {
            trackedPlayers.Remove(other.gameObject);
        }
    }
}

