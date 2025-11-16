using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Represents a tool that the player can use
[CreateAssetMenu(fileName = "NewTool", menuName = "Items/Tool")]
public class Tool : BaseItem
{
    public int durability;
    public int power;
}