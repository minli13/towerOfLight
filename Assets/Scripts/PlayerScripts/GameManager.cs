using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerMovement playerMovement;

    [Header("Game Progression Flags")]
    public bool inventoryEnabled = false;
    public bool craftingEnabled = false;
    public bool toolRingEnabled = false;

    [Header("Level Progression")]
    public bool level1Opened = false; // Temporary set true now in Shopkeeper after tutorial around line 181
    public bool level2Opened = false;

    [Header("Tutorial Settings")]
    public bool showTutorialPopups = true;
    public bool hasCraftedFirstTool = false;

    // Events for game progression
    public static event System.Action OnFirstCraftCompleted;
    public static event System.Action<bool> OnInventoryAccessChanged;
    public static event System.Action<bool> OnCraftingAccessChanged;
    public static event System.Action<bool> OnToolRingAccessChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Start with basic systems disabled until tutorial
        SetInventoryAccess(false);
        SetCraftingAccess(false);
        SetToolRingAccess(false);

    }

    public void LockInput()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }

    public void UnlockInput()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    public void CompleteFirstCraft()
    {
        if (!hasCraftedFirstTool)
        {
            hasCraftedFirstTool = true;
            OnFirstCraftCompleted?.Invoke();
        }
    }

    // Inventory Control Methods
    public void SetInventoryAccess(bool enabled)
    {
        inventoryEnabled = enabled;
        OnInventoryAccessChanged?.Invoke(enabled);

        if (enabled)
        {
            ShowTutorialMessage("Press I to open your inventory and manage items!");
        }
    }

    public void EnableInventoryAfterTutorial()
    {
        SetInventoryAccess(true);
    }

    // Crafting Control Methods
    public void SetCraftingAccess(bool enabled)
    {
        craftingEnabled = enabled;
        OnCraftingAccessChanged?.Invoke(enabled);

        if (enabled)
        {
            ShowTutorialMessage("You can now use crafting stations to create tools and items!");
        }
    }

    // Tool Ring Control Methods
    public void SetToolRingAccess(bool enabled)
    {
        toolRingEnabled = enabled;
        OnToolRingAccessChanged?.Invoke(enabled);

        if (enabled)
        {
            ShowTutorialMessage("Press Tab to open your tool ring and select tools!");
        }
    }

    // Tutorial Methods
    private void ShowTutorialMessage(string message)
    {
        if (showTutorialPopups)
        {
            Debug.Log($"TUTORIAL: {message}");

            // Optional: Lock input during tutorial messages
            // LockInput();
            // Show UI popup, then UnlockInput() when dismissed
        }
    }

    // Progress to next game stage
    public void CompleteTutorialStage(string stageName)
    {
        switch (stageName.ToLower())
        {
            case "inventory":
                SetInventoryAccess(true);
                break;
            case "crafting":
                SetCraftingAccess(true);
                break;
            case "tools":
                SetToolRingAccess(true);
                break;
            case "all":
                SetInventoryAccess(true);
                SetCraftingAccess(true);
                SetToolRingAccess(true);
                break;
        }
    }

    // Check if systems are available
    public bool CanUseInventory() => inventoryEnabled;
    public bool CanUseCrafting() => craftingEnabled;
    public bool CanUseToolRing() => toolRingEnabled;

    // Combined method for UI scenes (dialogs, inventory, etc.)
    public void SetUIState(bool uiOpen)
    {
        if (uiOpen)
        {
            LockInput();
            Time.timeScale = 0f; // Pause game time
        }
        else
        {
            UnlockInput();
            Time.timeScale = 1f; // Resume game time
        }
    }
}