using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Ghost : MonoBehaviour
{
    public int scoreValue = 5;
    public bool isVulnerable = false;

    public float fleeDistance = 5f;      // How far to flee when vulnerable
    public float updateRate = 0.2f;      // How often to update target
    public float moveSpeed = 3.5f;       // Adjustable movement speed

    private NavMeshAgent agent;
    public GameObject currentTarget;
    public List<GameObject> trackedPlayers = new List<GameObject>();
    private float timer = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed; // Set initial speed
    }

    void Update()
    {
        if (PauseManager.IsPaused)
        {
            agent.isStopped = true; // stop NavMeshAgent
            return;
        }
        else
        {
            agent.isStopped = false;
        }

        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            if (!isVulnerable)
            {
                UpdateCurrentTarget();  // pick closest
                ChaseCurrentTarget();
            }
            else
            {
                FleeFromPlayers();
            }
            timer = 0f;
        }
    }


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
        if (collision.gameObject.CompareTag("Player"))
        {
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
    }

    public void Die()
    {
        isVulnerable = false;
        transform.position = GhostManager.Instance.GetSpawnPosition();
    }

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
