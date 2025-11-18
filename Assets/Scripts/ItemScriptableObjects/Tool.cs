using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;
// Represents a tool that the player can use
[CreateAssetMenu(fileName = "NewTool", menuName = "Items/Tool")]
public class Tool : BaseItem
{
    public int durability;
    public int power;
  
}