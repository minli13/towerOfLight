using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaterialSlotPrefab : MonoBehaviour
{
    public Image icon;
    public TMP_Text itemName;
    public TMP_Text countText;

    // Fill the slot with recipe data
    public void Setup(BaseItem item, int ownedAmount, int requiredAmount)
    {
        icon.sprite = item.icon;
        itemName.text = item.itemName;
        countText.text = $"{ownedAmount} / {requiredAmount}";
    }
}
