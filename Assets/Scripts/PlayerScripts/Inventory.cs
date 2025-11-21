using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Header("Inventory Settings")]
    public int maxSlots = 8;

    [Header("UI Elements")]
    public GameObject inventoryPanel;
    public GameObject statisticsPanel;
    public GameObject interactionPanel;
    public Transform slotsParent;
    public GameObject slotPrefab;

    // Inventory data
    public Dictionary<BaseItem, int> items = new Dictionary<BaseItem, int>();
    public List<InventorySlot> slots = new List<InventorySlot>();

    // Events
    public static event Action<BaseItem> OnToolCrafted;
    public static event Action<BaseItem, Sprite> OnToolAddedToRing;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        InitializeSlots();
        UpdateInventoryUI();
        if (interactionPanel == null)
        {
            GameObject interactionPanel = GameObject.FindWithTag("InteractionPanel");
        }
        statisticsPanel = GameObject.Find("StatsPanel");
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false); // Hide inventory UI at start
        }
        // Ensure statistics panel starts inactive (since inventory starts closed)
        if (statisticsPanel != null)
        {
            statisticsPanel.SetActive(false);
        }
        // Ensure interaction panel starts active  
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(true);
        }
    }

    void Update()
    {
        if (!GameManager.Instance.CanUseInventory() && Input.GetKeyDown(KeyCode.I))
        {
            return;
        }
        // Press I to toggle inventory if allowed
        if (GameManager.Instance.CanUseInventory() && Input.GetKeyDown(KeyCode.I))
        {
            if (interactionPanel == null)
            {

                interactionPanel = GameObject.FindWithTag("InteractionPanel");
            }
            interactionPanel.SetActive(false); // Hide interaction panel when inventory is opened

            if (statisticsPanel == null)
            {
                statisticsPanel = GameObject.Find("StatsPanel");
            }
            statisticsPanel.SetActive(false); // Hide statistics panel when inventory is opened
            ToggleInventory();
        }

    }

    private void InitializeSlots()
    {
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }
        slots.Clear();

        // Create inventory slots
        for (int i = 0; i < maxSlots; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotsParent);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
            slots.Add(slot);
        }
    }

    // Close button to hide inventory
    public void CloseInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

    public void AddItem(BaseItem item, int quantity = 1)
    {
        if (items.ContainsKey(item))
        {
            items[item] += quantity;
        }
        else
        {
            items[item] = quantity;
        }

        Debug.Log($"Added {quantity} {item.itemName} to inventory");

        // Update UI
        UpdateInventoryUI();

        // If it's a tool, notify systems
        if (item.isTool == true)
        {
            // Enable tool ring usage
            OnToolCrafted?.Invoke(item);
            OnToolAddedToRing?.Invoke(item, item.icon);
        }

        // Check for inventory overflow
        if (items.Count > maxSlots)
        {
            Debug.LogWarning("Inventory is full!");
            return;
        }
    }

    public void UpdateInventoryUI()
    {
        // Clear all slots
        foreach (InventorySlot slot in slots)
        {
            if (slot != null)
            {
                slot.ClearSlot();
            }
        }
        // Populate slots with current items
        int index = 0;
        foreach (var itemPair in items)
        {
            if (index < slots.Count && slots[index] != null)
            {
                slots[index].SetSlot(itemPair.Key, itemPair.Value);
                index++;
            }
        }
    }

    public void ToggleInventory()
    {
        if (!GameManager.Instance.CanUseInventory())
        {
            return;
        }

        if (inventoryPanel != null)
        {
            bool newState = !inventoryPanel.activeInHierarchy;
            inventoryPanel.SetActive(newState);

            // Hide statistics panel when inventory is open
            if (statisticsPanel != null)
            {
                statisticsPanel.SetActive(!newState);
            }
            // Hide interaction panel when inventory is open
            if (interactionPanel != null)
            {
                interactionPanel.SetActive(!newState);
            }

            if (newState)
            {
                UpdateInventoryUI();
            }
            
        }
    }

    // Use consumable items
    public void UseItem(BaseItem item)
    {
        if (item == null || !HasItems(item, 1))
        {
            return;
        }

        if (item.isTool)
        {
            Debug.Log("Cannot use tool items directly from inventory.");
        }
        else if (item.isConsumable)
        {
            UseConsumable(item);
        }
        else
        {
            Debug.Log("Item is neither a tool nor a consumable.");
        }
    }

    private void UseConsumable(BaseItem item)
    {
        if (item.itemName.ToLower().Contains("apple") || item.restoreHealth > 0)
        {
            if (PlayerHealth.Instance != null)
            {
                int healAmount = item.restoreHealth > 0 ? item.restoreHealth : 1; // Default heal amount is 1 if not specified
                if (PlayerHealth.Instance.IsFullHealth())
                {
                    Debug.Log("Health is already full. Cannot use healing item.");
                    return;
                }
                PlayerHealth.Instance.Heal(healAmount);
                RemoveItems(item, 1);
                UpdateInventoryUI();
                Debug.Log($"Used {item.itemName}, restored {healAmount} health.");
            }
            else
            {
                Debug.Log("PlayerHealth instance not found.");
            }
        }
        else
        {
            Debug.Log("Consumable item has no defined use.");
        }
    }


    public bool HasItems(BaseItem item, int quantity)
    {
        return items.ContainsKey(item) && items[item] >= quantity;
    }

    public void RemoveItems(BaseItem item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            items[item] -= quantity;
            if (items[item] <= 0)
            {
                items.Remove(item);
            }
            Debug.Log($"Removed {quantity} {item.itemName} from inventory");
        }
    }

    public bool HasIngredients(CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (!HasItems(ingredient.item, ingredient.quantity))
            {
                return false;
            }
        }
        return true;
    }

    public void RemoveIngredients(CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            RemoveItems(ingredient.item, ingredient.quantity);
        }
    }

    public int GetItemCount(BaseItem item)
    {
        return items.ContainsKey(item) ? items[item] : 0;
    }
}
