using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Manages the conduit puzzle interaction
public class ConduitPuzzle : MonoBehaviour
{
    public bool isPowered = false;
    public SpriteRenderer conduitSprite;
    public Color poweredColor = Color.yellow;
    public Color unpoweredColor = Color.gray;
    private PuzzleUIManager puzzleUI;

    private void Start()
    {
        if (conduitSprite != null)
        {
            conduitSprite = GetComponent<SpriteRenderer>();
        }
        puzzleUI = FindObjectOfType<PuzzleUIManager>();
        UpdateConduitVisual();
    }

    private void OnMouseDown()
    {
        // Only interact if player has Screwdriver equipped
        if (ToolRingManager.Instance != null && ToolRingManager.Instance.IsToolEquipped("Screwdriver"))
        {
            if (!puzzleUI.IsPuzzleActive())
            {
                puzzleUI.ShowPuzzle();
            }
            isPowered = !isPowered;
            
            Debug.Log($"Conduit is now {(isPowered ? "powered" : "unpowered")}");
        }
        else
        {
            Debug.Log("You need to equip the Screwdriver to interact with the conduit.");
            // current tool equipped
            Debug.Log($"Current tool: {ToolRingManager.Instance.currentToolName}");

        }
    }

    public void OnPuzzleCompleted()
    {
        isPowered = true;
        UpdateConduitVisual();
        // Add 3 cell energy
        PlayerInventory.instance.AddCollectible();
        PlayerInventory.instance.AddCollectible();
        PlayerInventory.instance.AddCollectible();
        Debug.Log("Conduit powered! Added 3 cells to inventory.");
    }

    void UpdateConduitVisual()
    {
        conduitSprite.color = isPowered ? poweredColor : unpoweredColor;
    }
}
