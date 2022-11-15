using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : MoveState
{
    private bool started;
    private float speed;
    private const float DEFAULT_SPEED = 6f;
    private Vector3 moveDirection;
    
    public RollState(float rollSpeed = DEFAULT_SPEED)
    {
        ControlBlockLevel = ControlRestriction.All;
        AnimationBlockLevel = AnimationRestriction.All;

        started = false;
        speed = rollSpeed;
    }

    public override void Execute()
    {
        if (player == null)
        {
            Debug.LogError("RollState: Player reference was null");
            completed = true;
            return;
        }

        if (!started)
        {
            PlayRollAnimation();
            started = true;
        }

        player.Rigidbody.velocity = moveDirection * speed;
    }

    private void PlayRollAnimation()
    {
        moveDirection = player.MoveDirection;
        if (moveDirection == Vector3.zero)
        {
            moveDirection = player.LookDirection;
        }

        sm.CalculateDirection(moveDirection);
        sm.Action = AnimAction.Roll;
        sm.BodyPart = AnimBodyPart.Full;
        sm.UpdateSprite();
    }
}
