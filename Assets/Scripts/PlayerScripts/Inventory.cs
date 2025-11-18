using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<BaseItem, int> items = new Dictionary<BaseItem, int>();

    // Event to notify when tools are added
    public static event Action<BaseItem> OnToolCrafted;
    public static event Action<BaseItem, Sprite> OnToolAddedToRing;

    void Update()
    {
        // For testing: Press I to print inventory contents
        // TODO: Turn this into a proper UI display
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Inventory Contents:");
            foreach (var kvp in items)
            {
                Debug.Log($"{kvp.Key.itemName}: {kvp.Value}");
            }
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

        // If it's a tool, notify systems
        if (item.isTool == true) 
        {
            // Enable tool ring usage
            // ToolRingManager.Instance.toolRingPanel.SetActive(true);
            OnToolCrafted?.Invoke(item);
            OnToolAddedToRing?.Invoke(item, item.icon);
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
