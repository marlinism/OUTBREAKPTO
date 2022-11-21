using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponInventoryManager : MonoBehaviour
{
    public int weaponSlots = 3;
    public DamageSource projectileSource = DamageSource.Neutral;

    private LinkedList<GameObject> weaponList;
    private LinkedListNode<GameObject> current;

    private bool weaponsEnabled;

    public bool WeaponsEnabled
    {
        get { return weaponsEnabled; }
        set
        {
            if (value == true)
            {
                EnableWeapons();
            }
            else
            {
                DisableWeapons();
            }
        }
    }
    public WeaponManager CurrentWeapon
    {
        get
        {
            if (current == null || current.Value == null)
            {
                return null;
            }

            return current.Value.GetComponent<WeaponManager>();
        }
    }

    public int WeaponCount
    {
        get { return weaponList.Count; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        weaponList = new();
        current = null;
        weaponsEnabled = true;
    }

    public void AimCurrentWeapon(Vector2 aimDirection)
    {
        if (current == null || current.Value == null)
        {
            return;
        }

        current.Value.GetComponent<WeaponManager>().Aim(aimDirection);
    }

    public void AimCurrentWeapon(Vector2 aimDirection, Vector2 aimSpot)
    {
        if (current == null || current.Value == null)
        {
            return;
        }

        current.Value.GetComponent<WeaponManager>().Aim(aimDirection, aimSpot);
    }

    public void FireCurrentWeapon(bool triggerStarted)
    {
        if (current == null || current.Value == null)
        {
            return;
        }

        current.Value.GetComponent<WeaponManager>().Fire(triggerStarted);
    }

    // Rotate the current weapon to the next weapon
    public void RotateNextWeapon()
    {
        if (!weaponsEnabled)
        {
            return;
        }

        if (current == null)
        {
            current = weaponList.First;
            EnableCurrent();
            return;
        }

        DisableCurrent();
        current = current.Next;
        if (current == null)
        {
            current = weaponList.First;
        }
        EnableCurrent();
    }

    // Rotate the current weapon to the previous weapon
    public void RotatePrevWeapon()
    {
        if (!weaponsEnabled)
        {
            return;
        }

        if (current == null)
        {
            current = weaponList.First;
            EnableCurrent();
            return;
        }

        DisableCurrent();
        current = current.Previous;
        if (current == null)
        {
            current = weaponList.Last;
        }
        EnableCurrent();
    }

    // Add a weapon to the inventory 
    // Replaces the current weapon if inventory is full
    public bool AddWeapon(GameObject weapon)
    {
        if (weapon.tag != "Weapon")
        {
            return false;
        }

        if (weaponList.Count < weaponSlots)
        {
            weaponList.AddLast(weapon);
            DisableCurrent();
            current = weaponList.Last;
        }
        else
        {
            DisableCurrent();
            current.Value.GetComponent<WeaponManager>().Unequip();
            current.Value = weapon;
        }

        WeaponManager wm = current.Value.GetComponent<WeaponManager>();
        wm.Equip(gameObject);
        wm.projectileSource = projectileSource;

        if (weaponsEnabled)
        {
            EnableCurrent();
        }
        else
        {
            DisableCurrent();
        }

        return true;
    }

    // Remove the indicated weapon from the inventory
    public void RemoveWeapon(GameObject weapon)
    {
        if (weaponList.Count == 0)
        {
            return;
        }

        if (current.Value == weapon)
        {
            current.Value.GetComponent<WeaponManager>().Unequip();
            current = null;
            weaponList.Remove(weapon);
            current = weaponList.First;

            if (weaponsEnabled)
            {
                EnableCurrent();
            }
            else
            {
                DisableCurrent();
            }
            return;
        }

        LinkedListNode<GameObject> target = weaponList.Find(weapon);
        if (target == null)
        {
            return;
        }
        target.Value.GetComponent<WeaponManager>().Unequip();
        weaponList.Remove(weapon);
    }

    public void RemoveCurrentWeapon()
    {
        if (current == null || current.Value == null)
        {
            return;
        }

        current.Value.GetComponent<WeaponManager>().Unequip();
        weaponList.Remove(current);
        current = weaponList.First;

        if (weaponsEnabled)
        {
            EnableCurrent();
        }
        else
        {
            DisableCurrent();
        }
    }

    public void EnableWeapons()
    {
        weaponsEnabled = true;
        EnableCurrent();
    }

    public void DisableWeapons()
    {
        weaponsEnabled = false;
        DisableCurrent();
    }

    private void EnableCurrent()
    {
        if (current == null || current.Value == null)
        {
            return;
        }

        current.Value.GetComponent<WeaponManager>().Enable();
    }

    private void DisableCurrent()
    {
        if (current == null || current.Value == null)
        {
            return;
        }

        current.Value.GetComponent<WeaponManager>().Disable();
    }
}
