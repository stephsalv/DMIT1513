using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("State Manager")]
    public StateManager stateManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (stateManager == null || stateManager.currentState == null)
            return;

        // Example: trigger "Open" only when in IdleState
        if (stateManager.currentState is IdleState)
        {
            animator.SetBool("isPatrolling", true);
        }
        //else
        //{
        //    animator.SetBool("Open", false);
        //}
        if (stateManager.currentState is ChaseState)
        {
            animator.SetBool("isChasing", true);
        }
        else
        {
            animator.SetBool("isChasing", false);
        }

        if (stateManager.currentState is CryingState)
        {
            animator.SetBool("isCrying", true);
        }
        else
        {
            animator.SetBool("isCrying", false);
        }
    }
}
