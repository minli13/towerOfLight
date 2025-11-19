using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Manages the conduit puzzle interaction
public class ConduitPuzzle : MonoBehaviour
{
    public bool isPowered = false;
    public SpriteRenderer conduitSprite;
    public Color poweredColor = Color.yellow;
    public Color unpoweredColor = Color.gray;
    private PuzzleUIManager puzzleUI;
    public TextMeshProUGUI interactionMessage;
    private bool playerInRange = false;

    private void Start()
    {
        if (conduitSprite != null)
        {
            conduitSprite = GetComponent<SpriteRenderer>();
        }
        puzzleUI = FindObjectOfType<PuzzleUIManager>();
        UpdateConduitVisual();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            CheckForEquippedToolToStart();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press E to enter the tower!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    // Check if the player has the required tool equipped to start the puzzle
    private void CheckForEquippedToolToStart()
    {
        // Only interact if player has Screwdriver equipped
        if (ToolRingManager.Instance != null && ToolRingManager.Instance.IsToolEquipped("Screwdriver"))
        {
            if (!puzzleUI.IsPuzzleActive())
            {
                puzzleUI.ShowPuzzle();
            }
            isPowered = !isPowered;

            Debug.Log($"Conduit is now {(isPowered ? "powered" : "unpowered")} by {ToolRingManager.Instance.currentToolName}");
        }
        else
        {
            Debug.Log("You need to equip the Screwdriver to interact with the conduit.");
            interactionMessage.text = "Equip the Screwdriver to interact with the conduit.";
            // current tool equipped
            Debug.Log($"Current tool: {ToolRingManager.Instance.currentToolName}");

        }
    }

    public void OnPuzzleCompleted()
    {
        isPowered = true;
        UpdateConduitVisual();
        // Add 3 cell energy
        PlayerInventory.Instance.AddCollectible();
        PlayerInventory.Instance.AddCollectible();
        PlayerInventory.Instance.AddCollectible();
        // Message to player
        interactionMessage.text = "Conduit powered! Added 3 cells to inventory.";
        Debug.Log("Conduit powered! Added 3 cells to inventory.");
    }

    void UpdateConduitVisual()
    {
        conduitSprite.color = isPowered ? poweredColor : unpoweredColor;
    }
}
