using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the player's tools, allowing equipping and cycling through them
public class ToolSystem : MonoBehaviour
{
    public Tool[] availableTools; // tools the player owns
    public Tool currentTool; // currently selected tool

    private int currentIndex = 0;

    void Start()
    {
        if (availableTools.Length > 0)
        {
            EquipTool(0);
        }
    }
    void Update()
    {
        // Cycle through tools with Q and E keys
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CycleTool(-1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CycleTool(1);
        }
    }
    public void EquipTool(int index)
    {
       if (index >= 0 && index < availableTools.Length)
       {
            currentTool = availableTools[index];
            currentIndex = index;
            Debug.Log($"Equipped tool: {currentTool.toolName}");

        }
    }


    void CycleTool(int direction)
    {
        int newIndex = (currentIndex + direction + availableTools.Length) % availableTools.Length;
        EquipTool(newIndex);
    }

    public bool IsEquipped(string toolName)
    {
        return currentTool != null && currentTool.toolName == toolName;
    }
}
