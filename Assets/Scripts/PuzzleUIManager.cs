using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Manages the UI for the wire puzzle in the game
public class PuzzleUIManager : MonoBehaviour
{
    public GameObject wirePuzzlePanel;
    private bool isActive = false;

    void Start()
    {
        if (wirePuzzlePanel != null)
        {
            wirePuzzlePanel.SetActive(false);
        }
    }

    public void ShowPuzzle()
    { 
        if (wirePuzzlePanel != null)
        {
            wirePuzzlePanel.SetActive(true);
        }
        isActive = true;
        Time.timeScale = 0f; // Pause the game while solving
    }

    public void HidePuzzle()
    {
        if (wirePuzzlePanel != null)
        {
            wirePuzzlePanel.SetActive(false); 
        }
        isActive = false;
        Time.timeScale = 1f; // Resume the game
    }

    public bool IsPuzzleActive()
    {
        return isActive;
    }
}
