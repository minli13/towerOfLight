using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Ingredient
{
    public BaseItem item;
    public int amount;
}

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Game/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public BaseItem resultItem;
    public Ingredient[] ingredients;
}
