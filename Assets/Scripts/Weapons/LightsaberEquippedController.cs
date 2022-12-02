using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LightsaberEquippedController : WeaponEquippedController
{
    [SerializeField]
    private Hitbox hitbox;

    [SerializeField]
    private GameObject saberLight;

    protected override void Awake()
    {
        base.Awake();

        Assert.IsNotNull(hitbox);
        Assert.IsNotNull(saberLight);
    }

    // Aim the weapon towards the given direction
    public override void Aim(Vector2 aimDirection)
    {
        if (!weaponEnabled)
        {
            return;
        }

        SetDirection(aimDirection);
        saberLight.transform.position = (Vector2)holder.transform.position + aimDirection;
    }

    // Aim the weapon towards the given point.
    // Lightsaber does not need point aiming, so just use normal aim
    public override void Aim(Vector2 aimDirection, Vector2 aimPoint)
    {
        Aim(aimDirection);
    }

    // Swing lighsaber
    public override bool Fire()
    {
        if (!weaponEnabled)
        {
            return false;
        }

        // Check rate of fire and ammo
        if (Time.time - lastFireTime < wm.rateOfFire)
        {
            return false;
        }

        StartCoroutine(HitboxCoroutine());
        lastFireTime = Time.time;
        wsm.PlayFireAnim();
        return true;
    }

    private IEnumerator HitboxCoroutine()
    {
        hitbox.Enable();

        float startTime = Time.time;
        while (Time.time - startTime < 0.29)
        {
            hitbox.transform.position = transform.position;
            hitbox.transform.right = transform.right;
            yield return null;
        }

        hitbox.Disable();
        yield break;
    }

    private void OnDisable()
    {
        hitbox.Disable();
    }

    public override void Enable()
    {
        base.Enable();

        saberLight.SetActive(true);
    }

    public override void Disable()
    {
        base.Disable();

        transform.localPosition = Vector3.zero;
        saberLight.transform.localPosition = new Vector2(0.5f, 0.5f);
        saberLight.SetActive(false);
    }
}
