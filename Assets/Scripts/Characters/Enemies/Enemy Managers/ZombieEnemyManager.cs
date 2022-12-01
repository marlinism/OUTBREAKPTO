using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ZombieEnemyManager : Enemy
{
    [SerializeField]
    private EnemySpriteManager esm;
    [SerializeField]
    private MoveStateManager msm;
    [SerializeField]
    private WeaponInventoryManager wim;
    [SerializeField]
    private Hitbox tempHitbox;
    [SerializeField]
    private List<GameObject> weaponList;

    public float fireRange = 8f;
    public float meleeRange = 1f;

    // Properties
    public WeaponInventoryManager WeaponInventory
    {
        get { return wim; }
    }
    public EnemySpriteManager Sprite
    {
        get { return esm; }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        Assert.IsNotNull(esm);
        Assert.IsNotNull(msm);
        Assert.IsNotNull(wim);
        Assert.IsNotNull(tempHitbox);
        Assert.IsNotNull(weaponList);

        GetStartingWeapon();
        esm.CalculateOrientation(FaceDirection);
        esm.PlayAnimation("idle");
        wim.AimCurrentWeapon(FaceDirection);
    }

    // Update is called once per frame
    void Update()
    {
        if (!alerted)
        {
            Idle();
            return;
        }

        float distance = DistanceFromPlayer();
        FaceTowardsPlayer();
        esm.CalculateOrientation(faceDirection);

        msm.Execute();

        if (distance <= meleeRange)
        {
            // stub, do melee attack
            
        }
        else if (distance <= fireRange && SightlineToPlayer() && wim.WeaponCount > 0)
        {
            StopMovement();
            msm.AddMoveState(new ZombieFireState(gameObject));
        }
        else
        {
            ApproachPlayer();
        }
    }

    // Damageable Kill() method implementation
    public override void Kill()
    {
        // stub, add death animation
        RemoveWeapon();
        Destroy(gameObject);
    }

    public void RemoveWeapon()
    {
        if (wim.WeaponCount <= 0)
        {
            return;
        }

        WeaponManager weapon = wim.CurrentWeapon;
        wim.RemoveCurrentWeapon();
        Destroy(weapon.gameObject);

        // temporary solution to melee damage
        // Remove once headbutt attack is added
        tempHitbox.Enable();
    }

    private void GetStartingWeapon()
    {
        GameObject weapon = weaponList[Random.Range(0, weaponList.Count)];
        weapon = Instantiate(weapon);
        wim.AddWeapon(weapon);
    }

    public void Idle()
    {
        if (msm.ControlBlockLevel.HasFlag(ControlRestriction.Move))
        {
            return;
        }

        esm.PlayAnimation("idle");
    }

    public void ApproachPlayer()
    {
        if (msm.ControlBlockLevel.HasFlag(ControlRestriction.Move))
        {
            return;
        }

        FaceTowardsPlayer();
        MoveTowardsFaceDirection();
        esm.PlayAnimation("walk");
    }
}
