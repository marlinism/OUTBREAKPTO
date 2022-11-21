using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UISystem : MonoBehaviour
{
    // Singleton instance
    private static UISystem instance;

    private int prevMaxHealth;

    [SerializeField]
    private UIEffectsController uiEffects;

    // Component references
    [SerializeField]
    private HealthBarManager healthBar;

    // Instance property
    public static UISystem Inst
    {
        get { return instance; }
    }

    // Effect property
    public UIEffectsController Effects
    {
        get { return uiEffects; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Assert.IsNotNull(instance);
        Assert.IsNotNull(uiEffects);
        Assert.IsNotNull(healthBar);

        prevMaxHealth = 0;
    }

    // Update the UI health bar given the player's current's status
    public void UpdateHealthBar()
    {
        PlayerManager player = PlayerSystem.Inst.GetPlayerManager();
        if (player == null)
        {
            return;
        }

        healthBar.SetFillPosition((float)player.Health / (float)player.MaxHealth);

        if (player.MaxHealth != prevMaxHealth)
        {
            healthBar.IncreaseWidth(player.MaxHealth - prevMaxHealth);
            prevMaxHealth = player.MaxHealth;
        }
    }

    // Increase the size of the UI health bar
    public void IncreaseHealthBar(float increaseWidth)
    {
        healthBar.IncreaseWidth(increaseWidth);
    }
}
