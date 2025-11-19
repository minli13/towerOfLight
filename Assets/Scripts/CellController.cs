using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellController : MonoBehaviour
{
    public static CellController instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        PlayerInventory.Instance.UpdateCounterDisplay(); // Continuously update the UI
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player")) // Ensure your player GameObject has the "Player" tag
        {
            PlayerInventory.Instance.AddCollectible();
            PlayerInventory.Instance.UpdateCounterDisplay(); // Update the UI
            Destroy(gameObject); // Make the sprite disappear
        }
    }

}
