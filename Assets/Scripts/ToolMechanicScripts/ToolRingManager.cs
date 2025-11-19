using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolRingManager : MonoBehaviour
{
    public static ToolRingManager Instance;
    public ToolHolderController toolHolder;
    public GameObject toolRingPanel;
    public List<Image> toolIcons; 
    public Image selectionHighlight;
    public KeyCode openKey = KeyCode.Tab;

    [Header("Current Tool Info")]
    public string currentToolName;
    private int selectedToolIndex = 0;
    public bool isOpen = false;

    [Header("Tool Ring State")]
    public bool hasAnyTools = false; // flag to mark true when player gets first tool

    [Header("Tool Ring UI")]
    public Transform toolIconsParent; // Parent where tool icons will be created
    public GameObject toolIconPrefab; // Prefab for creating new tool icons

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        toolRingPanel.SetActive(false);

        // Initialize toolIcons as empty list if null
        if (toolIcons == null) toolIcons = new List<Image>();

        // Start with no tools
        hasAnyTools = false;
    }

    void Update()
    {
        // Only allow tool ring if player has tools
        if (!hasAnyTools)
        {
            return;
        }

        // Only allow if game state permits
        if (!GameManager.Instance.CanUseToolRing() && Input.GetKeyDown(KeyCode.Tab))
        {
            return;
        }
        if (GameManager.Instance.CanUseToolRing())
        {
            if (Input.GetKeyDown(openKey))
            {
                OpenRing();
            }

            if (Input.GetKeyUp(openKey))
            {
                CloseRing();
            }
        }

        // Navigate tools while ring is open
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetKeyDown(KeyCode.D)))
            {
                NextTool();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetKeyDown(KeyCode.A)))
            {
                PreviousTool();
            }
        }
    }

    void OpenRing()
    {
        // Double check we have tools
        if (!hasAnyTools)
        {
            return;
        }

        isOpen = true;
        toolRingPanel.SetActive(true);
        Time.timeScale = 0f; // Pause game


        // Hide the tool in hand while ring is open
        if (toolHolder != null && toolHolder.toolRenderer != null)
        {
            toolHolder.toolRenderer.enabled = false;
        }

        UpdateSelection();
    }

    void CloseRing()
    {
        // Double check we are open
        if (!isOpen)
        {
            return;
        }

        isOpen = false;
        toolRingPanel.SetActive(false);
        Time.timeScale = 1f; // Resume game

        if (toolIcons.Count > 0 && selectedToolIndex < toolIcons.Count && toolIcons[selectedToolIndex] != null)
        {
            currentToolName = toolIcons[selectedToolIndex].name;

            // Update the tool holder with the selected tool
            UpdateToolHolderWithSelectedTool();
        }
    }

    void NextTool()
    {
        if (toolIcons.Count == 0)
        {
            return;
        }
        selectedToolIndex = (selectedToolIndex + 1) % toolIcons.Count;
        UpdateSelection();
    }

    void PreviousTool()
    {
        if (toolIcons.Count == 0)
        {
            return;
        }
        selectedToolIndex = (selectedToolIndex - 1 + toolIcons.Count) % toolIcons.Count;
        UpdateSelection();
    }

    void UpdateSelection()
    {
        if (toolIcons.Count == 0 || selectionHighlight == null)
        {
            return;
        }
        // Move highlight to selected tool icon
        if (selectedToolIndex < toolIcons.Count && toolIcons[selectedToolIndex] != null)
        {
            selectionHighlight.transform.position = toolIcons[selectedToolIndex].transform.position;
        }
 
    }

    // Update the tool holder with currently selected tool
    private void UpdateToolHolderWithSelectedTool()
    {
        if (toolHolder != null && toolHolder.toolRenderer != null &&
            toolIcons.Count > 0 && selectedToolIndex < toolIcons.Count &&
            toolIcons[selectedToolIndex] != null)
        {
            toolHolder.toolRenderer.sprite = toolIcons[selectedToolIndex].sprite;
            toolHolder.toolRenderer.enabled = true;
        }

    }

    // Add crafted tool directly to the ring
    public void AddCraftedToolToRing(BaseItem tool, Sprite toolSprite)
    {
        // Create new tool icon UI element
        if (toolIconPrefab != null && toolIconsParent != null)
        {
            GameObject newToolIconObj = Instantiate(toolIconPrefab, toolIconsParent);
            Image newToolIcon = newToolIconObj.GetComponent<Image>();

            if (newToolIcon != null)
            {
                // Setup the new tool icon
                newToolIcon.sprite = toolSprite;
                newToolIcon.name = tool.itemName;
                // newToolIcon.gameObject.SetActive(true);

                // Add to our tracking list if not already present
                if (!toolIcons.Contains(newToolIcon))
                {
                    toolIcons.Add(newToolIcon);
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }

        // Refresh the circle arrangement
        ArrangeInCircle arrangeScript = toolRingPanel.GetComponentInChildren<ArrangeInCircle>();
        if (arrangeScript != null)
        {
            arrangeScript.RefreshArrangement();
        }

        // Enable the tool ring system when first tool is added
        if (!hasAnyTools)
        {
            hasAnyTools = true;

            // Auto-select the first tool
            selectedToolIndex = 0;
            currentToolName = tool.itemName;
        }

    }

    public bool IsToolEquipped(string toolName)
    {
        return currentToolName == toolName;
    }

    public bool HasTool(string toolName)
    {
        foreach (Image icon in toolIcons)
        {
            if (icon.name == toolName && icon.gameObject.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }
}