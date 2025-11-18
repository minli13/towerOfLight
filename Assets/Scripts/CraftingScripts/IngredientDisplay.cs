using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientDisplay : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Color hasEnoughColor = Color.white;
    [SerializeField] private Color notEnoughColor = Color.red;

    public void SetIngredient(BaseItem item, int requiredQuantity, int playerQuantity, bool hasEnough)
    {
        iconImage.sprite = item.icon;
        nameText.text = item.itemName;
        quantityText.text = $"{playerQuantity}/{requiredQuantity}";

        quantityText.color = hasEnough ? hasEnoughColor : notEnoughColor;
    }
}
