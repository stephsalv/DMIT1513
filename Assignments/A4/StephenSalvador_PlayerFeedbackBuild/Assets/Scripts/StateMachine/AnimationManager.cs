using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator animator;
    public StateManager stateManager;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (stateManager == null || stateManager.currentState == null)
            return;

        bool isPatrolling = stateManager.currentState is IdleState;
        bool isChasing = stateManager.currentState is ChaseState;
        bool isAttacking = stateManager.currentState is AttackState;
        bool isCrying = stateManager.currentState is CryingState;

        animator.SetBool("isPatrolling", isPatrolling);
        animator.SetBool("isChasing", isChasing);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isCrying", isCrying);
    }
}
