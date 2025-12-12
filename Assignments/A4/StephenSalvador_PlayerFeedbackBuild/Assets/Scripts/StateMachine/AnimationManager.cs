using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("State Manager")]
    public StateManager stateManager;

    [Header("Animation Prefabs")]
    public GameObject patrolEffectPrefab;
    public GameObject chaseEffectPrefab;
    public GameObject cryingEffectPrefab;

    // Track previous frame states
    private bool prevPatrolling = false;
    private bool prevChasing = false;
    private bool prevCrying = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (stateManager == null || stateManager.currentState == null)
            return;

        bool isPatrolling = stateManager.currentState is IdleState;
        bool isChasing = stateManager.currentState is ChaseState;
        bool isCrying = stateManager.currentState is CryingState;

        // Update animator
        animator.SetBool("isPatrolling", isPatrolling);
        animator.SetBool("isChasing", isChasing);
        animator.SetBool("isCrying", isCrying);

        // --- SPAWN PREFABS ON TRANSITION ---

        // Patrolling
        if (isPatrolling && !prevPatrolling && patrolEffectPrefab != null)
        {
            Instantiate(patrolEffectPrefab, transform.position, Quaternion.identity);
        }

        // Chasing
        if (isChasing && !prevChasing && chaseEffectPrefab != null)
        {
            Instantiate(chaseEffectPrefab, transform.position, Quaternion.identity);
        }

        // Crying
        if (isCrying && !prevCrying && cryingEffectPrefab != null)
        {
            Instantiate(cryingEffectPrefab, transform.position, Quaternion.identity);
        }

        // Save state for next frame
        prevPatrolling = isPatrolling;
        prevChasing = isChasing;
        prevCrying = isCrying;
    }
}
