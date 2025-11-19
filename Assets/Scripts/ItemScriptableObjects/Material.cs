using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMaterial", menuName = "Items/Material")]
public class Material : BaseItem
{
    public bool isStackable;
    public int maxStackSize;

}
