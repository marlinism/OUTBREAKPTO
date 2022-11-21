using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlinchState : MoveState
{
    private PlayerManager player;

    public FlinchState(GameObject caller) : base(caller)
    {
        ControlBlockLevel = ControlRestriction.None;
        PriorityLevel = 1;
        EqualOverwritten = true;
    }

    protected override void Initialize(GameObject caller)
    {
        player = caller.GetComponent<PlayerManager>();

        // Play flinch animation
        player.Sprite.Action = AnimAction.Flinch;
        player.Sprite.BodyPart = AnimBodyPart.Top;
        player.Sprite.PlaySequenceAnimation();
    }

    protected override void Execution()
    {
        return;
    }

    protected override void Restore()
    {
        player.Sprite.OverrideSequences();
    }
}
