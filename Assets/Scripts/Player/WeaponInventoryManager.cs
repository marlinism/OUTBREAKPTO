using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponInventoryManager : MonoBehaviour
{
    public PlayerManager player;

    public int weaponSlots = 3;

    private LinkedList<GameObject> weaponList;
    private LinkedListNode<GameObject> current;

    // Start is called before the first frame update
    void Awake()
    {
        Assert.IsNotNull(player);

        weaponList = new();
        current = null;
    }

    // Update the weapon state for the current weapon in the inventory
    public void UpdateCurrentWeaponState()
    {
        if (current == null || current.Value == null)
        {
            return;
        }

        current.Value.GetComponent<WeaponManager>().UpdateWeaponState(player.LookDirection);
    }

    // Rotate the current weapon to the next weapon
    public void RotateNextWeapon()
    {
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
            current.Value.GetComponent<WeaponManager>().Equip(gameObject);
        }
        else
        {
            DisableCurrent();
            current.Value.GetComponent<WeaponManager>().Unequip();
            current.Value = weapon;
            current.Value.GetComponent<WeaponManager>().Equip(gameObject);
        }
        EnableCurrent();

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
            EnableCurrent();
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
        EnableCurrent();
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
