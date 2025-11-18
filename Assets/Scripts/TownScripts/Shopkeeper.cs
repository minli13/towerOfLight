using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    public DialogueSequence firstTimeDialogue;  // ScriptableObject or array of strings
    public DialogueSequence repeatDialogue;     // Later: open shop window

    public BaseItem metalScrap;
    public BaseItem copperWire;
    public BaseItem apple;

    public int metalAmount = 3;
    public int copperAmount = 2;
    public int appleAmount = 5;

    private bool hasMetPlayer = false;
    private bool isPlayerInRange = false;

    private Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();

    }

    void Update()
    {
        if (!isPlayerInRange) return;
        if (DialogueUI.Instance.IsDialogueRunning) return;

        // Interact button
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }

    void Interact()
    {
        TownGameManager.Instance.LockInput();

        if (!hasMetPlayer)
        {
            hasMetPlayer = true;
            DialogueUI.Instance.IsDialogueRunning = true;
            StartCoroutine(RunFirstTimeDialogue()); // Start first time dialogue coroutine
        }
        else
        {
            // Later: open shop UI
            StartCoroutine(DialogueUI.Instance.RunDialogue(repeatDialogue)); // Just run repeat dialogue
        }
    }

    IEnumerator RunFirstTimeDialogue()
    {
        yield return DialogueUI.Instance.RunDialogue(firstTimeDialogue); // Wait for dialogue to finish

        // Debug.Log("First time dialogue finished.");

        // Give starter items
        inventory.AddItem(metalScrap, metalAmount);
        inventory.AddItem(copperWire, copperAmount);
        inventory.AddItem(apple, appleAmount);

        yield return DialogueUI.Instance.ShowLine($"Received {appleAmount} Apples!");
        yield return DialogueUI.Instance.ShowLine($"Received {copperAmount} Copper Wires!");
        yield return DialogueUI.Instance.ShowLine($"Received {metalAmount} Metal Scraps!");

        yield return DialogueUI.Instance.ShowLine("There you go! Try crafting over at the table!");

        yield return DialogueUI.Instance.WaitForInputCoroutine();

        TownGameManager.Instance.UnlockInput();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isPlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isPlayerInRange = false;
    }
}
