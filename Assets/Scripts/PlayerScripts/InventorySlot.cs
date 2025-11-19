using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Slot Data")]
    public Image itemIcon;
    public TextMeshProUGUI quantityText;
    public Button useButton;

    [Header("Slot Colors")]
    public Color normalColor = Color.white;
    public Color emptyColor = new Color(1, 1, 1, 0.2f);
    
    private BaseItem currentItem;
    private int currentQuantity;
    private Image slotBackground;

    private void Awake()
    {
        // Get the slot background image
        slotBackground = GetComponent<Image>();
        if (slotBackground != null)
        {
            slotBackground.color = emptyColor;
        }

        if (useButton != null)
        {
            useButton.onClick.AddListener(OnUseButtonClick);
        }
        // Initialize as empty
        ClearSlot();
    }

    public void SetSlot(BaseItem item, int quantity)
    {
        currentItem = item;
        currentQuantity = quantity;

        if (itemIcon != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.gameObject.SetActive(true);
            itemIcon.color = Color.white;
        }

        if (quantityText != null)
        {
            quantityText.text = quantity > 1 ? quantity.ToString() : "";
            quantityText.gameObject.SetActive(quantity > 1);
        }

        if (slotBackground != null)
        {
            slotBackground.color = normalColor;
        }

        // Show use button only for consumables
        if (useButton != null)
        {
            useButton.gameObject.SetActive(item.isConsumable);
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        currentQuantity = 0;

        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.gameObject.SetActive(false);
        }

        if (quantityText != null)
        {
            quantityText.text = "";
            quantityText.gameObject.SetActive(false);
        }

        if (slotBackground != null)
        {
            slotBackground.color = emptyColor;
        }

        if (useButton != null)
        {
            useButton.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null)
            return;
        // Left click to use item
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (currentItem.isConsumable)
            {
                OnUseButtonClick();
            }
            else
            {
                ShowItemInfo();
            }
        }
        // Right click to show item info
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ShowItemInfo();
        }
    }

    public void OnUseButtonClick()
    {
        if (currentItem != null && currentItem.isConsumable)
        {
            Inventory.Instance.UseItem(currentItem);
        } else
        {
            Debug.Log("Item is not consumable or no item in slot.");
        }
    }

    private void ShowItemInfo()
    {
        if (currentItem != null)
        {
            string itemInfo = $"Item: {currentItem.itemName}\n";
            if (!string.IsNullOrEmpty(currentItem.description))
            {
                itemInfo += $"{currentItem.description}\n";
            }
        }
    }

    public void UpdateQuantity (int newQuantity)
    {
        currentQuantity = newQuantity;
        if (quantityText != null)
        {
            quantityText.text = newQuantity > 1 ? newQuantity.ToString() : "";
            quantityText.gameObject.SetActive(newQuantity > 1);
        }
      
    }

    // getter methods
    public BaseItem GetItem()
    {
        return currentItem;
    }
    
    public int GetQuantity()
    {
        return currentQuantity;
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }
}


