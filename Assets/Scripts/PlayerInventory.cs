using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public int collectedCount = 0;

    void Awake()
    {
        instance = this;
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
}

