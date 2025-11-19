using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Header("Tutorial Settings")]
    public string tutorialStage; // "inventory", "crafting", "tools"
    public bool triggerOnStart = false;
    public bool triggerOnTriggerEnter = true;
    [TextArea] public string tutorialMessage;

    private void Start()
    {
        if (triggerOnStart)
        {
            TriggerTutorial();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnTriggerEnter && other.CompareTag("Player"))
        {
            TriggerTutorial();
        }
    }
    private void TriggerTutorial()
    {
        if (!string.IsNullOrEmpty(tutorialStage))
        {
            GameManager.Instance.CompleteTutorialStage(tutorialStage);
        }
        if (!string.IsNullOrEmpty(tutorialMessage))
        {
            // Show the tutorial message to the player
            // UIManager.Instance.ShowTutorialMessage(tutorialMessage);
        }
        gameObject.SetActive(false); // Disable after triggering
    }

}
