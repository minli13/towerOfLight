using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftButton : MonoBehaviour
{
    public CraftingRecipe recipe;

    public void OnCraftButtonPressed()
    {
        if (CraftingManager.Instance.Craft(recipe))
        {
            Debug.Log("Crafted: " + recipe.resultItem.itemName);
        }
        else
        {
            Debug.Log("Not enough materials!");
        }
    }
}
