using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageSystem : MonoBehaviour
{
    private static StorageSystem instance;

    [SerializeField]
    private GameObject itemStorage;

    [SerializeField]
    private GameObject projectileStorage;

    [SerializeField]
    private GameObject effectStorage;

    public static StorageSystem Inst
    {
        get { return instance; }
    }

    // Awake initializer
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void StoreItem(GameObject item)
    {
        item.transform.parent = itemStorage.transform;
    }

    public void StoreProjectile(GameObject projectile)
    {
        projectile.transform.parent = projectileStorage.transform;
    }

    public void StoreEffect(GameObject effect)
    {
        effect.transform.parent = effectStorage.transform;
    }
}
