using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UISystem : MonoBehaviour
{
    // Singleton instance
    private static UISystem instance;

    [SerializeField]
    private UIEffectsController uiEffects;

    // Component references
    [SerializeField]
    private GameObject overlayCanvas;

    [SerializeField]
    private HealthBarManager healthBar;

    [SerializeField]
    private MessageBoxManager messageBox;

    [SerializeField]
    private AmmoCounterManager ammoCounter;

    // Instance property
    public static UISystem Inst
    {
        get { return instance; }
    }
    
    // Previous recorded max health of the player
    private int prevMaxHealth;


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

        Assert.IsNotNull(overlayCanvas);
        Assert.IsNotNull(uiEffects);
        Assert.IsNotNull(healthBar);
        Assert.IsNotNull(messageBox);

        prevMaxHealth = 0;
        HideUI();
    }

    public void ShowUI()
    {
        overlayCanvas.SetActive(true);
    }

    public void HideUI()
    {
        overlayCanvas.SetActive(false);
    }

    // Update the UI health bar given the player's current's status
    public void UpdateHealthBar()
    {
        PlayerManager player = PlayerSystem.Inst.GetPlayerManager();
        if (player == null)
        {
            return;
        }

        if (player.MaxHealth != prevMaxHealth)
        {
            healthBar.IncreaseWidth(player.MaxHealth - prevMaxHealth);
            prevMaxHealth = player.MaxHealth;
        }

        healthBar.SetFillPosition((float)player.Health / (float)player.MaxHealth);
    }

    public void UpdateAmmoCounter()
    {
        ammoCounter.UpdateCounter();
    }

    // Increase the size of the UI health bar
    public void IncreaseHealthBar(float increaseWidth)
    {
        healthBar.IncreaseWidth(increaseWidth);
    }

    // Show a message to the player 
    public void ShowMessage(string text)
    {
        messageBox.Show(text);
    }

    // Remove the shown message from the screen
    public void RemoveMessage()
    {
        messageBox.Hide();
    }
}
