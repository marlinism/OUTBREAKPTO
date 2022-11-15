using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Assertions;

public class WeaponManager : MonoBehaviour
{
    public string weaponName = null;
    public GameObject projectile = null;
    public GameObject muzzleFlash = null;
    public float inaccuracyAngle = 5f;
    public float projectileSpeed = 75f;
    public float projectileMaxDist = 100f;
    public float baseDamage = 10;
    public float rateOfFire = 0.2f;
    public bool automaticFire = true;
    public bool projectilePenetration = false;
    public int ammoCapacity = 10;
    public float frontBackRange = 0.4f;

    public SpriteRenderer sr = null;
    public WeaponEquippedController wec = null;
    public GameObject equippedState = null;
    public GameObject unequippedState = null;
    public GameObject worldWeaponStorage = null;
    public GameObject worldProjectileStorage = null;

    private bool equipped;
    private Sprite unequippedSprite;
    private PlayerManager player;

    public bool Equipped
    {
        get { return equipped; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(weaponName);
        Assert.IsNotNull(projectile);
        Assert.IsNotNull(sr);
        Assert.IsNotNull(wec);
        Assert.IsNotNull(equippedState);
        Assert.IsNotNull(unequippedState);

        player = null;

        wec = transform.Find("Equipped State").GetComponent<WeaponEquippedController>();
        Assert.IsNotNull(wec);

        equipped = false;
        unequippedSprite = sr.sprite;
    }

    public void UpdateWeaponState(Vector3 pointDirection)
    {
        if (!equipped)
        {
            return;
        }

        wec.UpdateWeaponState(pointDirection);
    }

    public bool Equip(GameObject caller)
    {
        if (caller.tag != "Weapon Inventory" || equipped)
        {
            return false;
        }

        WeaponInventoryManager wim = caller.GetComponent<WeaponInventoryManager>();
        player = wim.player;
        transform.parent = wim.transform;
        transform.localPosition = Vector3.zero;

        equippedState.SetActive(true);
        unequippedState.SetActive(false);
        sr.sprite = null;

        wec.Initialize(player);

        equipped = true;
        return true;
    }

    public void Unequip()
    {
        if (!equipped)
        {
            return;
        }

        if (worldWeaponStorage == null)
        {
            transform.parent = null;
        }
        else
        {
            transform.parent = worldWeaponStorage.transform;
        }

        wec.Remove();
        equippedState.SetActive(false);
        unequippedState.SetActive(true);
        sr.sprite = unequippedSprite;

        equipped = false;
    }

    public void Enable()
    {
        if (!equipped)
        {
            return;
        }

        wec.Enable();
    }

    public void Disable()
    {
        if (!equipped)
        {
            return;
        }

        wec.Disable();
    }
}
