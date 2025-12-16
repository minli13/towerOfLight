using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Manages the player's inventory of collectibles
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    public TextMeshProUGUI counterText; // Reference to UI Text element
    public int collectedCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        counterText = GameObject.Find("CellCount").GetComponent<TextMeshProUGUI>();
        // Update counter display on start if a counterText is assigned
        if (counterText != null)
        {
            counterText.text = PlayerInventory.Instance.collectedCount.ToString();
        }
    }

    public void AddCollectible()
    {
        collectedCount++;
        Debug.Log("Collectibles: " + collectedCount);
    }

    public bool HasEnoughCollectible(int required)
    {
        return collectedCount >= required;
    }

    public int MissingCollectibles(int required)
    {
        return Mathf.Max(0, required - collectedCount);
    }

    public void UpdateCounterDisplay()
    {
        if (counterText != null)
        {
            counterText.text = PlayerInventory.Instance.collectedCount.ToString();
        }
    }
}

