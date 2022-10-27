using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum WeaponOrientation
{
    RightFront,
    RightBack,
    Front,
    Back,
    LeftFront,
    LeftBack
}

public class WeaponController : MonoBehaviour
{
    public string weaponName = null;
    public GameObject projectile = null;
    public GameObject muzzleFlash = null;
    public float rateOfFire = 0.2f;
    public bool automaticFire = true;
    public int ammoCapacity = 10;
    public float frontBackRange = 0.4f;

    private float lastFireTime;
    private int currAmmo;

    private bool weaponEnabled;
    private WeaponOrientation orientation;

    private Transform normalFirePoint;
    private Transform flippedFirePoint;
    private Transform projectileParent;

    private PlayerManager player;
    private WeaponSpriteManager wsm;

    public bool WeaponEnabled
    {
        get { return weaponEnabled; }
        set
        {
            if (value)
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }
    }

    public WeaponOrientation Orientation
    {
        get { return orientation; }
        set
        {
            SetOrientation(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.transform.parent.GetComponent<PlayerManager>();
        Assert.IsNotNull(player);

        wsm = transform.Find("Sprite").GetComponent<WeaponSpriteManager>();
        Assert.IsNotNull(wsm);

        normalFirePoint = transform.Find("Fire Points/Normal");
        flippedFirePoint = transform.Find("Fire Points/Flipped");
        projectileParent = GameObject.Find("Projectiles").transform;

        Assert.IsNotNull(weaponName);

        weaponEnabled = false;
        orientation = WeaponOrientation.RightFront;
        lastFireTime = 0;
        currAmmo = ammoCapacity;
    }

    public void UpdateWeaponState(Vector3 pointDirection)
    {
        if (player.ControlBlockLevel.HasFlag(ControlRestriction.HoldWeapon))
        {
            weaponEnabled = false;
            wsm.Visible = false;
            return;
        }
        else if (!weaponEnabled)
        {
            weaponEnabled = true;
            wsm.Visible = true;
        }

        SetDirection(pointDirection);

        if (currAmmo <= 0)
        {
            return;
        }

        if (automaticFire && Input.GetKey(KeyCode.Mouse0))
        {
            Fire();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }
    }

    public bool Fire()
    {
        if (projectile == null || muzzleFlash == null)
        {
            Debug.LogError("WeaponController: Projectile or Muzzle Flash not initialized");
            return false;
        }

        if (Time.time - lastFireTime < rateOfFire || currAmmo <= 0)
        {
            return false;
        }
        
        GameObject bullet = Instantiate(projectile);
        GameObject flash = Instantiate(muzzleFlash);
        
        if (wsm.Flipped)
        {
            bullet.transform.position = flippedFirePoint.position;
            flash.transform.position = flippedFirePoint.position;
        }
        else
        {
            bullet.transform.position = normalFirePoint.position;
            flash.transform.position = normalFirePoint.position;
        }
        bullet.transform.right = transform.right;
        bullet.transform.parent = projectileParent;

        lastFireTime = Time.time;
        --currAmmo;
        wsm.PlayFireAnim();
        return true;
    }

    public void SetDirection(Vector3 direction)
    {
        transform.right = direction;

        // Update orientation
        if (direction.y > 0)
        {
            if (Mathf.Abs(direction.x) <= frontBackRange)
            {
                SetOrientation(WeaponOrientation.Back);
            }
            else if (direction.x > 0)
            {
                SetOrientation(WeaponOrientation.RightBack);
            }
            else
            {
                SetOrientation(WeaponOrientation.LeftBack);
            }
        }
        else
        {
            if (Mathf.Abs(direction.x) <= frontBackRange)
            {
                SetOrientation(WeaponOrientation.Front);
            }
            else if (direction.x > 0)
            {
                SetOrientation(WeaponOrientation.RightFront);
            }
            else
            {
                SetOrientation(WeaponOrientation.LeftFront);
            }
        }
    }

    private void SetOrientation(WeaponOrientation inOrientation)
    {
        if (inOrientation == orientation)
        {
            return;
        }

        orientation = inOrientation;
        Transform pivot = transform.Find("Pivot Points/" + orientation.ToString());
        PivotPointData pivotData = pivot.GetComponent<PivotPointData>();

        transform.localPosition = pivot.localPosition;
        wsm.Flipped = pivotData.flipWeaponY;
        wsm.SortOrder = pivotData.LayerOrder;
    }

    public void Enable()
    {
        weaponEnabled = true;
        wsm.Visible = true;
    }

    public void Disable()
    {
        weaponEnabled = false;
        wsm.Visible = false;
    }
}
