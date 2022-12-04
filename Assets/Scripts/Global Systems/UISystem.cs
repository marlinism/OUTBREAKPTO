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

    [SerializeField]
    private Sprite crosshair;

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

    // Lock for modifying cursor visibility
    private bool cursorVisibilityLocked;

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

        if (crosshair != null)
        {
            Vector2 hotspot = new();
            hotspot.x = crosshair.texture.width / 2;
            hotspot.y = crosshair.texture.height / 2;
            Cursor.SetCursor(crosshair.texture, hotspot, CursorMode.Auto);
        }

        prevMaxHealth = 0;
        cursorVisibilityLocked = false;
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

    // Enable cursor visibility 
    public void ShowCursor()
    {
        if (cursorVisibilityLocked)
        {
            return;
        }

        Cursor.visible = true;
    }

    // Disable cursor visibility
    public void HideCursor()
    {
        if (cursorVisibilityLocked)
        {
            return;
        }

        Cursor.visible = false;
    }

    // Lock the current cursor visibility
    // Any ShowCursor() or HideCursor() will have to effect
    public void LockCursorVisibility()
    {
        cursorVisibilityLocked = true;
    }

    // Reallow modification of the cursor visibility
    public void UnlockCursorVisibility()
    {
        cursorVisibilityLocked = false;
    }

    // Update the UI ammo counter for the player's current ammo count
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
