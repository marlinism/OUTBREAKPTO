using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Hurtbox : MonoBehaviour
{
    [SerializeField]
    private Damageable damageTarget;

    [SerializeField]
    private bool ignoreNeutral;
    [SerializeField]
    private bool ignoreFriendly;
    [SerializeField]
    private bool ignoreHostile;

    private int ignoreMask;

    private void Awake()
    {
        Assert.IsNotNull(damageTarget);

        // Initialize ignore mask
        ignoreMask = 0;
        if (ignoreNeutral)
        {
            ignoreMask += (int)DamageSource.Neutral;
        }
        if (ignoreFriendly)
        {
            ignoreMask += (int)DamageSource.Friendly;
        }
        if (ignoreHostile)
        {
            ignoreMask += (int)DamageSource.Hostile;
        }
    }

    // Trigger enter function for Hitbox->Hurtbox collisions
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Hitbox hitbox = collision.GetComponent<Hitbox>();
        if (hitbox == null)
        {
            return;
        }

        if (((int)hitbox.Data.Source & ignoreMask) != 0)
        {
            return;
        }

        damageTarget.Damage(hitbox.Data, collision.gameObject);
    }

    // Recieve a Hit with a corresponding damage amount and optional
    // object that caused the hit
    public virtual void Hit(HitboxData damageInfo, GameObject collider = null)
    {
        if (((int)damageInfo.Source & ignoreMask) != 0)
        {
            return;
        }

        damageTarget.Damage(damageInfo, collider);
    }
}
