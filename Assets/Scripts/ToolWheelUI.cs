using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages the UI for the tool wheel, allowing players to see and select their tools
public class ToolWheelUI : MonoBehaviour
{
    public ToolSystem toolSystem;
    public GameObject wheelPanel;
    public Image[] toolIcons;

    void Start()
    {
        wheelPanel.SetActive(false); // Hide the wheel initially
        UpdateIcons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            wheelPanel.SetActive(!wheelPanel.activeSelf); // Show the wheel when Tab is pressed
        } 
    }

    void UpdateIcons()
    {
        for (int i = 0; i < toolIcons.Length && i < toolSystem.availableTools.Length; i++)
        {
            toolIcons[i].sprite = toolSystem.availableTools[i].icon;
        }
    }

}
