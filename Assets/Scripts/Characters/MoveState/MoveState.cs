using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Requirements for all derived MoveState classes:
//     - Define ControlBlockLevel and PriorityLevel
//     - Implement Initialize(), Execution(), and Restore()
//     - Restore() must revert any state changes in case of a priority overwrite

[System.Flags]
public enum ControlRestriction
{
    None        = 0,
    All         = ~0,
    Move        = (1 << 0),
    Look        = (1 << 1),
    Shoot       = (1 << 2),
    HoldWeapon  = (1 << 3)
}

public abstract class MoveState
{
    // Player control block level
    public ControlRestriction ControlBlockLevel
    {
        get; protected set;
    }

    // Priority level for overwritting
    // Higher priority states overwrite lower priority states
    // Must be 0 or greater
    public int PriorityLevel
    {
        get; protected set;
    }

    // Indicate if the state should be overwritten by equal priority states
    public bool EqualOverwritten
    {
        get; protected set;
    }

    // Indicate if the MoveState has completed execution
    public bool Completed
    {
        get; protected set;
    }

    // Reference to the GameObject actor this MoveState belongs to
    private GameObject actor;

    // Indicates if the MoveState has been initialized
    private bool initialized;

    // Constructor
    public MoveState(GameObject caller)
    {
        PriorityLevel = 0;
        EqualOverwritten = false;
        Completed = false;

        actor = caller;
        initialized = false;
    }

    // Execute the given state on an Update() basis
    // Should be called in Actor's Update() function
    public void Execute()
    {
        if (!initialized)
        {
            Initialize(actor);
            initialized = true;
        }

        Execution();
    }

    // Restores the actor state from any modifications made by the MoveState
    public void Finish()
    {
        if (!initialized)
        {
            return;
        }

        Restore();
    }

    // Notify the MoveState of a specific event
    public void Notify()
    {
        if (!initialized)
        {
            return;
        }

        Notification();
    }

    // Initializer function that is called before the first Execution() call
    protected abstract void Initialize(GameObject caller);

    // Executed on an Update() basis to perform the MoveState implementation
    protected abstract void Execution();

    // Executed when the MoveState is completed by the MoveStateManager
    // Must revert any state changes to cover priority overrides
    protected abstract void Restore();

    // Executed when the MoveState recieves a notification
    protected virtual void Notification()
    {
        return;
    }


}
