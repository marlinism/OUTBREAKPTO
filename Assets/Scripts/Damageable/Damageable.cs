using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Damageable : MonoBehaviour
{
    [SerializeField]
    protected int startingHealth = 100;

    protected int maxHealth;
    protected int currHealth;

    // Indicates if the Damageable object does not take damage
    public bool Invincible
    {
        get; set;
    }

    // Properties
    public int StartingHealth
    {
        get { return startingHealth; }
    }
    public int MaxHealth
    {
        get { return maxHealth; }
    }
    public int Health
    {
        get { return currHealth; }
    }

    protected virtual void Start()
    {
        maxHealth = startingHealth;
        currHealth = startingHealth;
        Invincible = false;
    }

    // Abstract Kill method
    // Request for the Damageable object to the killed/destroyed
    // Should be called from Damage() when currHealth <= 0
    public abstract void Kill();

    // Abstract RecieveDamage method
    // Call for the Damagable object to take a certain amount of damage
    public abstract void RecieveDamage(HitboxData damageInfo, GameObject collider = null);

    // Damage interface method
    // Used by outside objects to signal for the Damageable class to be damaged
    public virtual void Damage(HitboxData damageInfo, GameObject collider = null)
    {
        if (Invincible)
        {
            return;
        }

        RecieveDamage(damageInfo, collider);
    }

    // Heal method
    // Heal the damageable object by the given amount of health
    public virtual void Heal(int healAmount)
    {
        currHealth += healAmount;

        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }
    }

    // IncreaseHealth method
    // Increases the max health of the damageable object
    public virtual void IncreaseHealth(int increaseAmount)
    {
        maxHealth += increaseAmount;
    }
}
