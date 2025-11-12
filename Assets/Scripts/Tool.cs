using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Represents a tool that the player can use
[CreateAssetMenu(fileName = "NewTool", menuName = "Items/Tool")]
public class Tool : ScriptableObject
{
    public string toolName;
    public Sprite icon;
}