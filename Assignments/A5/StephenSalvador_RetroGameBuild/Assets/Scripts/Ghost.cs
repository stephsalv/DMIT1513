using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Ghost : MonoBehaviour
{
    public int scoreValue = 5;
    public bool isVulnerable = false;

    public float fleeDistance = 5f;      // How far to flee when vulnerable
    public float updateRate = 0.2f;      // How often to update target

    private NavMeshAgent agent;
    public GameObject currentTarget;
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
                ChaseClosestPlayer();
            }
            else
            {
                FleeFromPlayers();
            }
            timer = 0f;
        }
    }

    private void ChaseClosestPlayer()
    {
        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;

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

        currentTarget = closestPlayer;

        if (currentTarget != null)
            agent.SetDestination(currentTarget.transform.position);
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

        agent.SetDestination(transform.position + fleeDirection * fleeDistance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                if (isVulnerable)
                {
                    player.score += scoreValue;
                    Die();
                }
                else
                {
                    player.Die();
                }
            }
        }
    }

    public void Die()
    {
        isVulnerable = false;
        transform.position = GhostManager.Instance.GetSpawnPosition();
    }
}
