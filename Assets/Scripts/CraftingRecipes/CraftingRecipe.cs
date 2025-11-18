using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public Sprite icon;
    public BaseItem resultItem; // Replaced GameObject with BaseItem
    public int resultQuantity = 1;

    [System.Serializable]
    public class Ingredient
    {
        public BaseItem item; // Now uses BaseItem instead of separate Item class
        public int quantity;
    }

    public Ingredient[] ingredients;
}

