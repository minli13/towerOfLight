using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolRingManager : MonoBehaviour
{
    public static ToolRingManager Instance;
    public GameObject toolRingPanel; // Panel containing the tool ring UI
    public List<Image> toolIcons; // UI Images for tool icons
    public Image selectionHighlight; // Highlight for selected tool
    public KeyCode openKey = KeyCode.Tab; // Key to open tool ring

    [Header("Current Tool Info")]
    public string currentToolName; // TODO: set default tool in story !!!!
    private int selectedToolIndex = 0;
    private bool isOpen = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        toolRingPanel.SetActive(false);
        UpdateSelection();
    }

    void Update()
    {
        if (Input.GetKeyDown(openKey))
        {
            OpenRing();
            Debug.Log("Tool ring opened");
        }

        if (Input.GetKeyUp(openKey))
        {
            CloseRing();
        }

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
        isOpen = true;
        toolRingPanel.SetActive(true);
        Time.timeScale = 0f; // pause game time
        UpdateSelection();
    }

    void CloseRing()
    {
        isOpen = false;
        toolRingPanel.SetActive(false);
        Time.timeScale = 1f; // resume game time
    
        currentToolName = toolIcons[selectedToolIndex].name;
        Debug.Log($"Selected tool index: {toolIcons[selectedToolIndex].name}");
    }

    void NextTool()
    {
        selectedToolIndex = (selectedToolIndex + 1) % toolIcons.Count;
        UpdateSelection();
    }

    void PreviousTool()
    {
        selectedToolIndex = (selectedToolIndex - 1 + toolIcons.Count) % toolIcons.Count;
        UpdateSelection();
    }

    void UpdateSelection()
    {
        selectionHighlight.transform.position = toolIcons[selectedToolIndex].transform.position;
    }

    public bool IsToolEquipped(string toolName)
    {
        return currentToolName == toolName;
    }
}
