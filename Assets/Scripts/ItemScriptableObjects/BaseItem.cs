using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool isTool; // For distinguishing materials from tools
    public bool isConsumable; // For consumable items
    public string description;
    public int restoreHealth;
}
