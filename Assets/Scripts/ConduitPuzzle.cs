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
        // Only interact if player has Pliers equipped
        ToolSystem toolSystem = FindObjectOfType<ToolSystem>();
        if (toolSystem != null && toolSystem.IsEquipped("Pliers"))
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
            Debug.Log("You need to equip the Pliers to interact with the conduit.");
        }
    }

    public void OnPuzzleCompleted()
    {
        isPowered = true;
        UpdateConduitVisual();
    }

    void UpdateConduitVisual()
    {
        conduitSprite.color = isPowered ? poweredColor : unpoweredColor;
    }
}
