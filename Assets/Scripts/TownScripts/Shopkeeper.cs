using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    [Header("Tutorial Settings")]
    // public DialogueSequence tutorialDialogue;
    private bool hasGivenTutorial = false;
    private bool isTutorialActive = false;
    private string waitingFor = ""; // What the tutorial is currently waiting for

    [Header("Dialogue Sequences")]
    public DialogueSequence firstTimeDialogue;
    public DialogueSequence repeatDialogue;

    [Header("Starter Items")]
    public BaseItem metalScrap;
    public BaseItem copperWire;
    public BaseItem apple;

    public int metalAmount = 3;
    public int copperAmount = 2;
    public int appleAmount = 5;

    [Header("Interaction Settings")]
    private bool hasMetPlayer = false;
    private bool isPlayerInRange = false;

    private Inventory inventory;
    private PlayerMovement playerMovement; // Reference to the actual movement script

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        playerMovement = FindObjectOfType<PlayerMovement>(); // Get the movement script

        // Start with all systems locked until tutorial
        GameManager.Instance.SetInventoryAccess(false);
        GameManager.Instance.SetCraftingAccess(false);
        GameManager.Instance.SetToolRingAccess(false);
        // Subscribe to first craft event
        GameManager.OnFirstCraftCompleted += OnFirstCraftCompleted;
    }

    private void OnDestroy()
    {
        GameManager.OnFirstCraftCompleted -= OnFirstCraftCompleted;
    }


    void Update()
    {
        if (!isPlayerInRange) return;
        if (DialogueUI.Instance.IsDialogueRunning) return;

        // Check for interaction input
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }

        // Check for tutorial trigger (player crafted first tool)
        if (!hasGivenTutorial && GameManager.Instance.hasCraftedFirstTool && !isTutorialActive)
        {
            TriggerTutorial();
        }

    }

    void Interact()
    {
        // Lock player movement
        LockPlayer();
        GameManager.Instance.LockInput();

        // Lock NPC patrols
        TownGameManager.Instance.LockInput();

        if (!hasMetPlayer)
        {
            hasMetPlayer = true;
            DialogueUI.Instance.IsDialogueRunning = true;
            StartCoroutine(RunFirstTimeDialogue());
        }
        else if (hasGivenTutorial)
        {
            DialogueUI.Instance.IsDialogueRunning = true;
            StartCoroutine(RunRepeatDialogue());
        }
        
        else if (!Inventory.Instance.inventoryPanel.activeInHierarchy)
        {
            // Tutorial not complete yet, show a message and unlock player
            DialogueUI.Instance.IsDialogueRunning = true;
            StartCoroutine(ShowTutorialLockedMessage());
        }
    }

    IEnumerator ShowTutorialLockedMessage()
    {   
        if (isTutorialActive)
        {
            yield return DialogueUI.Instance.ShowLine($"Hold on! Let's finish the tutorial first. Press {waitingFor}!");
            yield return DialogueUI.Instance.WaitForInputCoroutine();
        }
        else
        {
            yield return DialogueUI.Instance.ShowLine("Come back after you've tried crafting your first tool!");
            yield return DialogueUI.Instance.WaitForInputCoroutine();
            // Unlock the player since we're not running a full dialogue
            UnlockPlayer();
            GameManager.Instance.UnlockInput();
        }

        // Unlock the player since we're not running a full dialogue
        // UnlockPlayer();
        // GameManager.Instance.UnlockInput();

    }

    void OnFirstCraftCompleted()
    {
        Debug.Log("Shopkeeper detected first craft completion. Waiting to trigger tutorial.");
    }

    void TriggerTutorial()
    {
        if (hasGivenTutorial || isTutorialActive) return;
        isTutorialActive = true;
        // Lock player for tutorial
        LockPlayer();
        TownGameManager.Instance.LockInput();
        // Start tutorial dialogue
        StartCoroutine(RunTutorialDialogue());
    }


    IEnumerator RunFirstTimeDialogue()
    {
        yield return DialogueUI.Instance.RunDialogue(firstTimeDialogue);

        // Give starter items
        inventory.AddItem(metalScrap, metalAmount);
        inventory.AddItem(copperWire, copperAmount);
        inventory.AddItem(apple, appleAmount);

        // Notify player of received items
        yield return DialogueUI.Instance.ShowLine($"Received {appleAmount} Apples!");
        yield return DialogueUI.Instance.ShowLine($"Received {copperAmount} Copper Wires!");
        yield return DialogueUI.Instance.ShowLine($"Received {metalAmount} Metal Scraps!");
        
        // Enable crafting after first dialogue
        GameManager.Instance.SetCraftingAccess(true);
        yield return DialogueUI.Instance.ShowLine("There you go! Try crafting over at the table!");
        
        yield return DialogueUI.Instance.WaitForInputCoroutine();

        UnlockPlayer();
    }
    IEnumerator RunTutorialDialogue()
    {
        yield return DialogueUI.Instance.ShowLine("Hey! I see you've crafted your first tool!");

        // Tool Ring Tutorial
        GameManager.Instance.SetToolRingAccess(true);
        waitingFor = "TAB"; // tool ring
        yield return DialogueUI.Instance.ShowLine("Press TAB to open your tool ring...");
        yield return WaitForUIWindow(() => ToolRingManager.Instance.isOpen, "tool ring");
        yield return DialogueUI.Instance.ShowLine("Great! Use TAB to switch between tools.");

        yield return DialogueUI.Instance.WaitForInputCoroutine();

        // Inventory Tutorial  
        GameManager.Instance.SetInventoryAccess(true);
        waitingFor = "I"; // inventory
        yield return DialogueUI.Instance.ShowLine("Now press I to open your inventory...");
        yield return WaitForUIWindow(() => Inventory.Instance.inventoryPanel.activeInHierarchy, "inventory");
        yield return DialogueUI.Instance.ShowLine("Perfect! Your inventory stores all items.");
        yield return DialogueUI.Instance.WaitForInputCoroutine();

        yield return DialogueUI.Instance.ShowLine("You're all set! Good luck!");
        yield return DialogueUI.Instance.WaitForInputCoroutine();

        // Complete tutorial
        hasGivenTutorial = true;
        isTutorialActive = false;
        waitingFor = "";
        GameManager.Instance.level1Opened = true; // Unlock level 1
        UnlockPlayer();
    }

    // Helper method to wait for any UI window to open and close
    IEnumerator WaitForUIWindow(System.Func<bool> isOpenFunction, string windowName)
    {
        Debug.Log($"Waiting for {windowName} to open...");

        // Wait for window to open
        yield return new WaitUntil(isOpenFunction);
        Debug.Log($"{windowName} opened - waiting for close...");

        // Wait for window to close
        yield return new WaitUntil(() => !isOpenFunction());
        Debug.Log($"{windowName} closed - continuing...");
    }

    IEnumerator RunRepeatDialogue()
    {
        yield return DialogueUI.Instance.RunDialogue(repeatDialogue);
        yield return DialogueUI.Instance.WaitForInputCoroutine();

        UnlockPlayer();
    }

    // Helper method to lock everything
    private void LockPlayer()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            playerMovement.ForceStopMovement();
        }
        Input.ResetInputAxes(); // Clear any existing input states

        TownGameManager.Instance.LockInput();
        DialogueUI.Instance.IsDialogueRunning = true;


    }

    // Helper method to unlock everything
    private void UnlockPlayer()
    {
        // Unlock player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        // Unlock NPC patrols
        TownGameManager.Instance.UnlockInput();

        DialogueUI.Instance.IsDialogueRunning = false;
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