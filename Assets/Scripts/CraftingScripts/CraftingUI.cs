using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public static CraftingUI Instance;

    [Header("UI References")]
    public GameObject craftingPanel;
    public Transform recipeButtonParent; 
    public GameObject recipeButtonPrefab; 

    [Header("Selected Recipe Panel")]
    public Image selectedRecipeIcon;
    public TextMeshProUGUI selectedRecipeName;
    public Transform ingredientsParent;
    public GameObject ingredientDisplayPrefab;
    public Button craftButton;
    public TextMeshProUGUI craftButtonText;

    [Header("UI Navigation")]
    public Button closeButton;

    private CraftingStation currentStation;
    private CraftingRecipe selectedRecipe;
    private List<GameObject> currentRecipeButtons = new List<GameObject>();
    private List<GameObject> currentIngredientDisplays = new List<GameObject>();

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Optional: keep UI between scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        craftingPanel.SetActive(false);

        // Set up close button
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideCraftingUI);
        }
    }

    void Update()
    {
        // Allow escape key to close UI
        if (craftingPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            HideCraftingUI();
        }

        // Update recipe button appearances when UI is open
        if (craftingPanel.activeInHierarchy)
        {
            foreach (var buttonObj in currentRecipeButtons)
            {
                RecipeButton recipeButton = buttonObj.GetComponent<RecipeButton>();
                if (recipeButton != null)
                {
                    recipeButton.UpdateButtonAppearance();
                }
            }

            // Also update selected recipe display if we have one selected
            if (selectedRecipe != null)
            {
                UpdateSelectedRecipeDisplay();
            }
        }
    }

    public void ShowCraftingStation(CraftingStation station)
    {
        currentStation = station;
        craftingPanel.SetActive(true);
        PopulateRecipeButtons();
        ClearSelectedRecipe();

        // Pause game disable player movement
        Time.timeScale = 0f;
        GameManager.Instance.LockInput();
    }

    public void HideCraftingUI()
    {
        craftingPanel.SetActive(false);
        currentStation = null;

        // Resume game or enable player movement
        Time.timeScale = 1f;
        GameManager.Instance.UnlockInput();
    }

    private void PopulateRecipeButtons()
    {
        // Clear existing buttons
        foreach (var button in currentRecipeButtons)
        {
            Destroy(button);
        }
        currentRecipeButtons.Clear();

        // Create new buttons
        if (currentStation != null && currentStation.availableRecipes != null)
        {
            foreach (var recipe in currentStation.availableRecipes)
            {
                if (recipeButtonPrefab != null && recipeButtonParent != null)
                {
                    GameObject buttonObj = Instantiate(recipeButtonPrefab, recipeButtonParent);
                    RecipeButton recipeButton = buttonObj.GetComponent<RecipeButton>();
                    recipeButton.Initialize(recipe, this);
                    currentRecipeButtons.Add(buttonObj);
                }
            }
        }
    }

    public void SelectRecipe(CraftingRecipe recipe)
    {
        selectedRecipe = recipe;
        UpdateSelectedRecipeDisplay();
    }

    private void UpdateSelectedRecipeDisplay()
    {
        if (selectedRecipe == null) return;

        // Update basic info
        selectedRecipeIcon.sprite = selectedRecipe.icon;
        selectedRecipeName.text = selectedRecipe.recipeName;

        // Clear existing ingredient displays
        foreach (var display in currentIngredientDisplays)
        {
            Destroy(display);
        }
        currentIngredientDisplays.Clear();

        // Create ingredient displays
        Inventory inventory = FindObjectOfType<Inventory>();
        bool canCraft = true;

        foreach (var ingredient in selectedRecipe.ingredients)
        {
            GameObject ingredientObj = Instantiate(ingredientDisplayPrefab, ingredientsParent);
            IngredientDisplay display = ingredientObj.GetComponent<IngredientDisplay>();

            bool hasEnough = inventory.HasItems(ingredient.item, ingredient.quantity);
            int playerQuantity = inventory.GetItemCount(ingredient.item);

            display.SetIngredient(ingredient.item, ingredient.quantity, playerQuantity, hasEnough);

            currentIngredientDisplays.Add(ingredientObj);

            if (!hasEnough)
            {
                canCraft = false;
            }
        }

        // Update craft button
        craftButton.interactable = canCraft;
        craftButtonText.text = canCraft ? "Craft" : "Missing Materials";
    }

    public void CraftSelectedRecipe()
    {
        if (selectedRecipe == null) return;

        Inventory inventory = FindObjectOfType<Inventory>();

        if (inventory.HasIngredients(selectedRecipe))
        {
            inventory.RemoveIngredients(selectedRecipe);

            // Add the crafted item to inventory
            if (selectedRecipe.resultItem != null)
            {
                inventory.AddItem(selectedRecipe.resultItem, selectedRecipe.resultQuantity);
                ToolRingManager.Instance.AddCraftedToolToRing(selectedRecipe.resultItem, selectedRecipe.icon);
            }

            // Update UI
            UpdateSelectedRecipeDisplay();

            Debug.Log($"Crafted {selectedRecipe.resultQuantity} {selectedRecipe.recipeName}!");
        }
    }

    private void ClearSelectedRecipe()
    {
        selectedRecipe = null;
        selectedRecipeIcon.sprite = null;
        selectedRecipeName.text = "Select a Recipe";

        foreach (var display in currentIngredientDisplays)
        {
            Destroy(display);
        }
        currentIngredientDisplays.Clear();

        craftButton.interactable = false;
        craftButtonText.text = "Select Recipe";
    }
}