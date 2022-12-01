using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadMGState : MoveState
{
    private PlayerManager player;
    private WeaponManager wm;

    public ReloadMGState(GameObject caller, WeaponManager weapon) : base(caller)
    {
        wm = weapon;

        ControlBlockLevel = ControlRestriction.All;
        PriorityLevel = 2;
    }

    protected override void Initialize(GameObject caller)
    {
        player = caller.GetComponent<PlayerManager>();

        player.WeaponInventory.DisableWeapons();
        player.Reloading = true;
        player.Rigidbody.velocity = Vector2.zero;

        player.Sprite.Direction = AnimDirection.SideFront;
        player.Sprite.FlipX = false;
        player.Sprite.Action = AnimAction.ReloadMG;
        player.Sprite.BodyPart = AnimBodyPart.Full;
        player.Sprite.PlaySequenceAnimation();

        player.Sprite.Lock();
    }

    protected override void Execution()
    {
        if (!player.Reloading)
        {
            Completed = true;
            return;
        }
    }

    // Notification() is used to known when to reload the weapon
    protected override void Notification()
    {
        wm.Reload();
        UISystem.Inst.UpdateAmmoCounter();
    }

    protected override void Restore()
    {
        player.Sprite.OverrideSequences();
        player.Sprite.Unlock();
        player.WeaponInventory.EnableWeapons();
        player.Reloading = false;
    }
}
