using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Button button;

    [Header("State Colors")]
    [SerializeField] private Color canCraftColor = Color.white;
    [SerializeField] private Color cannotCraftColor = new Color(0.7f, 0.7f, 0.7f, 0.5f);

    private CraftingRecipe recipe;
    private CraftingUI craftingUI;
    private Inventory inventory;

    public void Initialize(CraftingRecipe newRecipe, CraftingUI ui)
    {
        recipe = newRecipe;
        craftingUI = ui;
        inventory = FindObjectOfType<Inventory>();

        iconImage.sprite = recipe.icon;
        nameText.text = recipe.recipeName;

        button.onClick.AddListener(OnClick);
        UpdateButtonAppearance();
    }

    private void OnClick()
    {
        craftingUI.SelectRecipe(recipe);
    }

    public void UpdateButtonAppearance()
    {
        bool canCraft = inventory.HasIngredients(recipe);
        backgroundImage.color = canCraft ? canCraftColor : cannotCraftColor;
    }
}
