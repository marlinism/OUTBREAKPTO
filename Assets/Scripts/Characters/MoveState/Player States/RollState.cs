using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : MoveState
{
    private PlayerManager player;
    private float speed;
    private const float DEFAULT_SPEED = 6f;
    private Vector3 moveDirection;
    
    public RollState(GameObject caller, float rollSpeed = DEFAULT_SPEED) : base(caller)
    {
        ControlBlockLevel = ControlRestriction.All;
        PriorityLevel = 5;

        speed = rollSpeed;
    }

    protected override void Initialize(GameObject caller)
    {
        player = caller.GetComponent<PlayerManager>();
        player.WeaponInventory.DisableWeapons();
        PlayRollAnimation();

        player.Invincible = true;
    }

    protected override void Execution()
    {
        player.Rigidbody.velocity = moveDirection * speed;
    }

    // Notification() is used to known when to stop providing i-frames
    protected override void Notification()
    {
        player.Invincible = false;
    }

    protected override void Restore()
    {
        player.Sprite.Unlock();
        player.Sprite.OverrideSequences();
        player.WeaponInventory.EnableWeapons();
        player.Invincible = false;
    }

    private void PlayRollAnimation()
    {
        moveDirection = player.MoveDirection;
        if (moveDirection == Vector3.zero)
        {
            moveDirection = player.LookDirection;
        }

        player.Sprite.CalculateDirection(moveDirection);
        player.Sprite.Action = AnimAction.Roll;
        player.Sprite.BodyPart = AnimBodyPart.Full;
        player.Sprite.PlaySequenceAnimation();
        player.Sprite.Lock();
    }
}
