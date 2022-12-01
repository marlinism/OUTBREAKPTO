using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFireState : MoveState
{
    private enum FireSubstate
    {
        Updating,
        Aiming,
        AimHolding,
        Shooting,
        Recovering,
        CheckAmmo
    }

    private ZombieEnemyManager zombie;

    // General substate info
    private FireSubstate substate = FireSubstate.Aiming;
    private float substateStart;

    // Aiming substate
    private static float aimTime = 0.4f;
    Vector2 startingDirection;

    // AimHolding substate
    private static float holdPause = 0.1f;

    // Shooting substate
    private static float automaticFireTime = 0.6f;

    // Recovery substate
    private static float recoveryTime = 0.4f;

    private bool automaticWeapon;

    public ZombieFireState(GameObject caller) : base(caller)
    {
        ControlBlockLevel = ControlRestriction.All;
        PriorityLevel = 1;
        EqualOverwritten = false;
    }

    protected override void Initialize(GameObject caller)
    {
        zombie = caller.GetComponent<ZombieEnemyManager>();
        substateStart = Time.time;

        automaticWeapon = zombie.WeaponInventory.CurrentWeapon.automaticFire;
    }

    protected override void Execution()
    {
        if (zombie.DistanceFromPlayer() > zombie.fireRange)
        {
            Completed = true;
            return;
        }

        if (PlayerSystem.Inst.GetPlayer() == null)
        {
            Completed = true;
            return;
        }

        switch(substate)
        {
            case FireSubstate.Updating:
                if (!zombie.SightlineToPlayer())
                {
                    Completed = true;
                }
                else if (zombie.WeaponInventory.CurrentWeapon.Ammo <= 0)
                {
                    zombie.RemoveWeapon();
                    Completed = true;
                }
                else
                {
                    startingDirection = zombie.FaceDirection;
                    substate = FireSubstate.Aiming;
                    substateStart = Time.time;
                }
                return;

            case FireSubstate.Aiming:
                float aimCompletion = (Time.time - substateStart) / aimTime;
                zombie.FaceDirection = Vector2.Lerp(startingDirection, zombie.DirectionTowardsPlayer(), aimCompletion);
                zombie.WeaponInventory.AimCurrentWeapon(zombie.FaceDirection);
                zombie.Sprite.CalculateOrientation(zombie.FaceDirection);
                zombie.Sprite.PlayAnimation("idle");
                if (Time.time - substateStart >= aimTime)
                {
                    substate = FireSubstate.AimHolding;
                    substateStart = Time.time;
                }
                return;

            case FireSubstate.AimHolding:
                if (Time.time - substateStart >= holdPause)
                {
                    substate = FireSubstate.Shooting;
                    substateStart = Time.time;
                }
                return;

            case FireSubstate.Shooting:
                zombie.WeaponInventory.FireCurrentWeapon(true);
                if (!automaticWeapon | Time.time - substateStart >= automaticFireTime)
                {
                    substate = FireSubstate.Recovering;
                    substateStart = Time.time;
                }
                return;

            case FireSubstate.Recovering:
                if (Time.time - substateStart >= recoveryTime)
                {
                    substate = FireSubstate.Updating;
                    substateStart = Time.time;
                }
                return;

            case FireSubstate.CheckAmmo:
                if (zombie.WeaponInventory.CurrentWeapon.Ammo <= 0)
                {
                    zombie.RemoveWeapon();
                    Completed = true;
                    return;
                }
                substate = FireSubstate.Updating;
                substateStart = Time.time;
                return;
        }
    }

    protected override void Restore()
    {
        return;
    }
}
