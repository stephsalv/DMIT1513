using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class Ghost : MonoBehaviour
{
    [Header("Gameplay")]
    public int scoreValue = 5;
    public bool isVulnerable = false;

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float fleeDistance = 5f;
    public float updateRate = 0.2f;

    [Header("Tracking")]
    public List<GameObject> trackedPlayers = new List<GameObject>();
    public GameObject currentTarget;

    private NavMeshAgent agent;
    private Renderer[] ghostRenderers;
    private Color originalColor;
    private float timer = 0f;
    private bool canMove = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        ghostRenderers = GetComponentsInChildren<Renderer>();
        if (ghostRenderers.Length > 0)
            originalColor = ghostRenderers[0].material.color;
    }

    private void Update()
    {
        if (!canMove) return;

        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            timer = 0f;

            if (isVulnerable)
            {
                FleeFromPlayers();
                SetGhostColor(Color.white);
            }
            else
            {
                ChaseClosestPlayer();
                SetGhostColor(originalColor);
            }
        }
    }

    private void UpdateCurrentTarget()
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (var player in trackedPlayers)
        {
            if (player == null) continue;

            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc == null || !pc.isAlive) continue;

            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        currentTarget = closestPlayer;
    }

    private void ChaseClosestPlayer()
    {
        UpdateCurrentTarget();
        if (currentTarget != null)
            agent.SetDestination(currentTarget.transform.position);
    }

    private void FleeFromPlayers()
    {
        if (trackedPlayers.Count == 0) return;

        Vector3 fleeDirection = Vector3.zero;
        int validPlayers = 0;

        foreach (var player in trackedPlayers)
        {
            if (player == null) continue;

            PlayerController pc = player.GetComponent<PlayerController>();
            if (pc == null || !pc.isAlive) continue;

            fleeDirection += (transform.position - player.transform.position).normalized;
            validPlayers++;
        }

        if (validPlayers == 0) return;

        fleeDirection /= validPlayers;
        Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

        if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, fleeDistance, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player == null || !player.isAlive) return;

        if (isVulnerable)
        {
            player.score += scoreValue;
            Respawn();
        }
        else
        {
            player.Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !trackedPlayers.Contains(other.gameObject))
            trackedPlayers.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            trackedPlayers.Remove(other.gameObject);
    }

    public void StopMovement()
    {
        canMove = false;
        if (agent != null)
            agent.isStopped = true;
    }

    private void SetGhostColor(Color color)
    {
        if (ghostRenderers == null) return;

        foreach (var rend in ghostRenderers)
        {
            rend.material.color = color;
        }
    }

    public void Respawn()
    {
        isVulnerable = false;
        transform.position = GhostManager.Instance.GetSpawnPosition();
    }
}
