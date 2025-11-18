using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<BaseItem, int> items = new Dictionary<BaseItem, int>();
    // Debug method to display inventory contents when pressing 'I'
    void Update()
    {
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

    // Helper method to get item count for UI
    public int GetItemCount(BaseItem item)
    {
        return items.ContainsKey(item) ? items[item] : 0;
    }
}
