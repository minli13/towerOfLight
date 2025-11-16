using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (!Inventory.Instance.HasEnough(ingredient.item, ingredient.amount))
                return false;
        }
        return true;
    }

    public bool Craft(CraftingRecipe recipe)
    {
        if (!CanCraft(recipe))
            return false;

        // Remove ingredients
        foreach (var ingredient in recipe.ingredients)
        {
            Inventory.Instance.Remove(ingredient.item, ingredient.amount);
        }

        // Add result
        Inventory.Instance.Add(recipe.resultItem, 1);

        return true;
    }
}

