using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellController : MonoBehaviour
{
    public TextMeshProUGUI counterText; // Reference to UI Text element
    public static CellController instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Update counter display on start if a counterText is assigned
        if (counterText != null)
        {
            counterText.text = "Cells: " + PlayerInventory.instance.collectedCount.ToString();
        }
    }

    private void Update()
    {
        UpdateCounterDisplay(); // Continuously update the UI
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player")) // Ensure your player GameObject has the "Player" tag
        {
            
            PlayerInventory.instance.AddCollectible();
            UpdateCounterDisplay(); // Update the UI
            Destroy(gameObject); // Make the sprite disappear
        }
    }
    public void UpdateCounterDisplay()
    {
        if (counterText != null)
        {
            counterText.text = "Cells: " + PlayerInventory.instance.collectedCount.ToString();
        }
    }
}
