using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingStation : MonoBehaviour
{
    public CraftingRecipe[] availableRecipes;

    [Header("Interaction Settings")]
    public float interactionRadius = 2f;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI Prompt")]
    public TMP_Text interactionPrompt;
    public GameObject interactionPanel;

    private bool playerInRange = false;
    private GameObject player;

    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Check for player input when in range
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            OpenCraftingUI();
        }
    }

    // Use OnTriggerEnter2D for 2D physics
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
            interactionPanel.SetActive(true);
            

            if (GameManager.Instance.CanUseCrafting() == false)
            {
                interactionPrompt.text = "Crafting Unavailable!";
                return;
            }
            // Show UI prompt
            else if (interactionPrompt != null)
            {
                interactionPrompt.text = "Press E to craft";
            }
            else
            {
                Debug.LogWarning("No interaction prompt assigned in inspector");
            }
        }
    }

    // Use OnTriggerExit2D for 2D physics
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            // Debug.Log("Player left crafting station range");

            // Hide UI prompt
            interactionPanel.SetActive(false);

            if (interactionPrompt != null)
            {
                interactionPrompt.text = "";
            }
        }
    }

    private void OpenCraftingUI()
    {
        // Check if crafting is allowed
        if (!GameManager.Instance.CanUseCrafting())
        {
            return;
        }
        if (CraftingUI.Instance != null)
        {
            CraftingUI.Instance.ShowCraftingStation(this);
            // Debug.Log("Crafting UI opened successfully");
        }
        else
        {
            Debug.LogError("CraftingUI.Instance is null.");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
