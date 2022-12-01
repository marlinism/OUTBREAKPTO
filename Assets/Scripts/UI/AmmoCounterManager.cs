using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AmmoCounterManager : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI currAmmoCount;

    [SerializeField]
    private TMPro.TextMeshProUGUI ammoCapacityCount;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(currAmmoCount);
        Assert.IsNotNull(ammoCapacityCount);
    }

    public void UpdateCounter()
    {
        PlayerManager player = PlayerSystem.Inst.GetPlayerManager();
        WeaponManager weapon = (player != null) ? player.WeaponInventory.CurrentWeapon : null;
        if (weapon == null)
        {
            currAmmoCount.text = "";
            ammoCapacityCount.text = "";
            return;
        }

        currAmmoCount.text = weapon.Ammo.ToString();
        ammoCapacityCount.text = weapon.AmmoCapacity.ToString();
    }
}
