using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// Manages the terminal that collects energy from the player
public class TerminalManager : MonoBehaviour
{
    private bool playerInRange = false;
    private PlayerInventory playerInventory;

    [Header("Energy Settings")]
    public int storedEnergy = 0;
    public int requiredEnergy = 5;
    public bool isActivated = false;

    [Header("UI")]
    public TextMeshProUGUI promptText;


    void Start()
    {
        
    }

    void Update()
    {
        // Only listen for key press if player is near
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isActivated)
        {
            TryTransferEnergy();
        }
    }

    void TryTransferEnergy()
    {
        if (playerInventory != null && playerInventory.collectedCount > 0)
        {
            int transferred = playerInventory.collectedCount;
            storedEnergy += transferred;
            playerInventory.collectedCount = 0;

            if (storedEnergy >= requiredEnergy && !isActivated)
            {
                ActivateTerminal();
                
            }

            if (!isActivated)
            {
                // Update prompt to show new hub status
                UpdatePrompt($"Transferred {transferred} cells. (Hub: {storedEnergy}/{requiredEnergy})");
            }

        }
        else
        {
            // Player has no energy — show temporary message
            UpdatePrompt("No energy to transfer!");
        }
    }

    void UpdatePrompt(string message)
    {
        if (promptText == null) return;
        promptText.text = message;
        Debug.Log(message);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<PlayerInventory>();

            if (!isActivated)
            {
                // Show the default prompt once when entering
                UpdatePrompt($"Press E to transfer energy.\n(Hub: {storedEnergy}/{requiredEnergy})");
            }
            else
            {
                UpdatePrompt("Terminal fully powered!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
            UpdatePrompt(""); // Clear prompt when player leaves
        }
    }

    void ActivateTerminal()
    {
        isActivated = true;
        storedEnergy = requiredEnergy;
        UpdatePrompt("Terminal fully powered!");
        // Additional activation logic can be added here (e.g., open doors, trigger events)
        // For example:
        // change terminal color to green
        GetComponent<SpriteRenderer>().color = Color.green;
    }
}

