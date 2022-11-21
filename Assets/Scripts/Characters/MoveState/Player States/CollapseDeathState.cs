using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseDeathState : MoveState
{
    private enum DeathSubstate
    {
        Starting,
        Flash,
        Final
    }

    private PlayerManager player;

    private DeathSubstate substate = DeathSubstate.Starting;
    private float pauseDuration = 1f;
    private float pauseTime = 0f;

    public CollapseDeathState(GameObject caller) : base(caller)
    {
        ControlBlockLevel = ControlRestriction.All;
        PriorityLevel = 10;
        EqualOverwritten = false;
    }

    protected override void Initialize(GameObject caller)
    {
        player = caller.GetComponent<PlayerManager>();
        player.Invincible = true;
        player.Rigidbody.velocity = Vector2.zero;
        player.WeaponInventory.DisableWeapons();

        // Play collapse death animation
        player.Sprite.OverrideSequences();
        player.Sprite.Unlock();
        player.Sprite.Action = AnimAction.CollapseDeath;
        player.Sprite.BodyPart = AnimBodyPart.Full;
        player.Sprite.Direction = AnimDirection.SideFront;
        player.Sprite.PlaySequenceAnimation();
        player.Sprite.Lock();

        // Slow time
        Time.timeScale = 0.4f;
    }

    protected override void Execution()
    {
        switch (substate)
        {
            case DeathSubstate.Starting:
                return;

            case DeathSubstate.Flash:
                if (Time.unscaledTime - pauseTime >= pauseDuration)
                {
                    Time.timeScale = 1f;
                    UISystem.Inst.Effects.DisableEffect(UIEffect.ScreenCrack);
                    player.SortGroup.sortingLayerName = "Active";
                    player.SortGroup.sortingOrder = 0;
                    substate = DeathSubstate.Final;
                }
                return;
        }
    }

    protected override void Notification()
    {
        switch(substate)
        {
            case DeathSubstate.Starting:
                UISystem.Inst.Effects.EnableEffect(UIEffect.ScreenCrack);
                pauseTime = Time.unscaledTime;
                Time.timeScale = 0f;
                player.SortGroup.sortingLayerName = "UI";
                player.SortGroup.sortingOrder = 10;
                substate = DeathSubstate.Flash;
                return;
        }
    }

    protected override void Restore()
    {
        player.Invincible = false;
        player.SortGroup.sortingLayerName = "Active";
        player.SortGroup.sortingOrder = 0;
        Time.timeScale = 1f;
        UISystem.Inst.Effects.DisableEffect(UIEffect.ScreenCrack);
        player.Sprite.OverrideSequences();

        // Blackscreen, todo: add death action
        UISystem.Inst.Effects.EnableEffect(UIEffect.BlackScreen);
        DebugSystem.Instance.DeathReload();
    }
}
