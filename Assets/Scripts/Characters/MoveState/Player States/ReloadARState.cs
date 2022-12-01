using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadARState : MoveState
{
    private PlayerManager player;
    private WeaponManager wm;

    public ReloadARState(GameObject caller, WeaponManager weapon) : base(caller)
    {
        wm = weapon;

        ControlBlockLevel = ControlRestriction.HoldWeapon;
        PriorityLevel = 2;
    }

    protected override void Initialize(GameObject caller)
    {
        player = caller.GetComponent<PlayerManager>();

        player.WeaponInventory.DisableWeapons();
        player.SpeedMultiplier = 0.75f;
        player.Reloading = true;

        player.Sprite.Direction = AnimDirection.SideFront;
        player.Sprite.FlipX = false;
        player.Sprite.Action = AnimAction.ReloadAR;
        player.Sprite.BodyPart = AnimBodyPart.Top;
        player.Sprite.PlaySequenceAnimation();

        player.Sprite.BodyPart = AnimBodyPart.Front;
        player.Sprite.PlaySequenceAnimation();

        player.Sprite.LockDirection();
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
        player.Sprite.UnlockDirection();
        player.Sprite.OverrideSequences();
        player.Sprite.DisableFront();
        player.WeaponInventory.EnableWeapons();
        player.SpeedMultiplier = 1f;
        player.Reloading = false;
    }
}
