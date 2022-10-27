using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MoveStateManager : MonoBehaviour
{
    // Current MoveState being executed
    private MoveState currentState;

    // Buffered MoveState to potentially execute
    private MoveState bufferState;
    private float bufferAddTime;

    // Time buffer for the buffered MoveState to be accepted
    public float bufferAcceptanceTime = 0.25f;

    // Block level properties
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
    public AnimationRestriction AnimationBlockLevel
    {
        get
        {
            if (currentState == null)
            {
                return AnimationRestriction.None;
            }
            return currentState.AnimationBlockLevel;
        }
    }

    // Component references
    private PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerManager>();
        Assert.IsNotNull(player);

        currentState = null;
        bufferState = null;
        bufferAddTime = 0f;
    }

    // Execute the current MoveState
    // Call this method in Update() if there is a MoveState
    public void Execute()
    {
        if (currentState == null)
        {
            return;
        }

        if (currentState.completed)
        {
            CompleteCurrentState();
        }

        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    // Add a MoveState as the current state
    // If there is already a state, the MoveState is buffered
    public void AddMoveState(MoveState state)
    {
        if (currentState == null)
        {
            currentState = state;
            currentState.Initialize(gameObject);
            return;
        }

        bufferState = state;
        bufferState.Initialize(gameObject);
        bufferAddTime = Time.time;
    }

    // Get the current MoveState
    public MoveState GetMoveState()
    {
        return currentState;
    }

    // Complete the current MoveState and remove it
    // The buffered MoveState will be set to current if present and within the buffer time
    public void CompleteCurrentState()
    {
        if (currentState == null)
        {
            return;
        }

        currentState.Completed();

        if (bufferState  == null)
        {
            currentState = null;
        }
        else if (Time.time - bufferAddTime <= bufferAcceptanceTime)
        {
            currentState = bufferState;
            bufferState = null;
            bufferAddTime = 0f;
        }
        else
        {
            currentState = null;
            bufferState = null;
            bufferAddTime = 0f;
        }

        // TODO: Restore old state
    }

    // Check if there is no MoveState currently being executed
    public bool Empty()
    {
        return currentState == null;
    }
}
