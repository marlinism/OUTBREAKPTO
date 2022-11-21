using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MoveStateManager : MonoBehaviour
{
    // Current MoveState being executed
    private MoveState currentState;

    // Control Block Level property
    public ControlRestriction ControlBlockLevel
    {
        get
        {
            if (currentState == null)
            {
                return ControlRestriction.None;
            }
            return currentState.ControlBlockLevel;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = null;
    }

    // Execute the current MoveState
    // Call this method in Update() if there is a MoveState
    public void Execute()
    {
        if (currentState == null)
        {
            return;
        }

        currentState.Execute();

        if (currentState.Completed)
        {
            FinishCurrentState();
        }
    }

    // Add a MoveState as the current state
    // If there is already a state, override if a higher priority
    public void AddMoveState(MoveState state)
    {
        if (currentState == null)
        {
            currentState = state;
            return;
        }

        if (state.PriorityLevel > currentState.PriorityLevel)
        {
            currentState.Finish();
            currentState = state;
            return;
        }

        if (state.PriorityLevel == currentState.PriorityLevel && currentState.EqualOverwritten)
        {
            currentState.Finish();
            currentState = state;
            return;
        }
    }

    // Get the current MoveState
    public MoveState GetMoveState()
    {
        return currentState;
    }

    // Notify the current state about some event
    public void NotifyCurrentState()
    {
        if (currentState == null)
        {
            return;
        }

        currentState.Notify();
    }

    // Finish the current MoveState and remove it
    // The buffered MoveState will be set to current if present and within the buffer time
    public void FinishCurrentState()
    {
        if (currentState == null)
        {
            return;
        }

        currentState.Finish();
        currentState = null;
    }

    // Check if there is no MoveState currently being executed
    public bool Empty()
    {
        return currentState == null;
    }
}
