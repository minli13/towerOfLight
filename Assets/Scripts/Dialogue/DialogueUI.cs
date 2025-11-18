using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("Typewriter Settings")]
    public float typeSpeed = 0.03f;     // delay between characters
    public bool skipTyping;             // used to skip the animation
    private string currentFullLine = ""; // to track the full line being typed

    [Header("UI References")]
    public GameObject panel;
    public TMP_Text dialogueText;

    private bool waitingForInput = false;
    public bool IsDialogueRunning = false;
    public bool isCutscene = false; // set to true if dialogue is part of a cutscene

    private void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        panel.SetActive(false);
    }

    // Called from scripts like NPC interaction
    public IEnumerator RunDialogue(DialogueSequence sequence)
    {
        panel.SetActive(true);
        TownGameManager.Instance?.LockInput();

        IsDialogueRunning = true;


        // Wait a frame to avoid input carryover
        // Determine if this is a cutscene dialogue
        if (!isCutscene)
        {
            yield return StartCoroutine(WaitForInputCoroutine()); 
        }
        

        foreach (string line in sequence.lines)
        {
            currentFullLine = line;

            // Typewriter animation
            yield return StartCoroutine(TypeLine(line));

            waitingForInput = true;

            // Debug.Log("DIALOGUE LINE: " + line);

            // Wait until player hits E or Space
            while (waitingForInput)
            {
                yield return StartCoroutine(WaitForInputCoroutine());
            }
        }
           
        IsDialogueRunning = false;
        panel.SetActive(false);
        TownGameManager.Instance?.UnlockInput();
    }

    public IEnumerator ShowLine(string line)
    {
        panel.SetActive(true);
        TownGameManager.Instance?.LockInput();

        IsDialogueRunning = true;

        // Wait a frame to avoid input carryover
        // Determine if this is a cutscene dialogue
        if (!isCutscene)
        {
            yield return StartCoroutine(WaitForInputCoroutine());
        }

        waitingForInput = true;

        currentFullLine = line;

        // Typewriter animation
        yield return StartCoroutine(TypeLine(line));

        while (waitingForInput) {
            yield return StartCoroutine(WaitForInputCoroutine());
        }
        IsDialogueRunning = false;
        panel.SetActive(false);
        TownGameManager.Instance?.UnlockInput();

    }

    public IEnumerator WaitForInputCoroutine()
    {
 
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            // Debug.Log("Input received, continuing dialogue.");

            // If still typing then skip
            if (!skipTyping && dialogueText.text != currentFullLine)
            {
                skipTyping = true;  // instantly show full line
            }
            else
            {
                waitingForInput = false; // move to next line
            }
        }
        yield return null;
        
    }

    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        skipTyping = false;

        foreach (char c in line)
        {
            // If player presses input, skip type animation
            if (skipTyping)
            {
                dialogueText.text = line;
                break;
            }

            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }


}

