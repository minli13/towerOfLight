using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
// Manages the UI for the wire puzzle in the game
public class PuzzleUIManager : MonoBehaviour
{
    [Header("Wire Puzzle UI")]
    public GameObject wirePuzzlePanel;
    private bool isActive = false;
    public TextMeshProUGUI interactionMessage;

    [Header("PuzzleDamageSettings")]
    public bool causeDamageOnFail = true;
    public int damageAmountOnFail = 1;

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
        Debug.Log("Hiding Wire Puzzle UI");
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

    public void OnPuzzleFailed()
    {
        if (causeDamageOnFail && PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.TakeDamage(damageAmountOnFail);

        }
        ResetPuzzle();
    }

    public void ResetPuzzle()
    {
        WirePuzzle wirePuzzle = FindObjectOfType<WirePuzzle>();
        if (wirePuzzle != null)
        {
            wirePuzzle.ResetPuzzle();
        }
        // Add reset logic here
        HidePuzzle();
        // Send message to player
        interactionMessage.text = "Puzzle failed! Try again.";
    }

}
