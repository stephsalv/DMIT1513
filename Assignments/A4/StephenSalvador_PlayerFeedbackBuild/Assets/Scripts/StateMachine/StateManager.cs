using UnityEngine;

public class StateManager : MonoBehaviour
{
    public State currentState;

    private void Start()
    {
        if (currentState != null)
            currentState.Enter();
    }

    private void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();

        // ONLY switch if the state actually changes
        if (nextState != null && nextState != currentState)
        {
            currentState.Exit();
            currentState = nextState;
            currentState.Enter();
        }
    }
}

