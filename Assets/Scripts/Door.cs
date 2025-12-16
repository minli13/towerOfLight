using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    [SerializeField] private string sceneToLoad = "TowerLevel1"; // target scene
    [SerializeField] private Vector2 spawnPosition; // optional spawn point in new scene

    [Header("UI Prompt")]
    public TMP_Text interactionPrompt;
    public GameObject interactionPanel;

    private bool playerInRange = false;

    void Update()
    {
        // If not unlocked, do nothing
        if (GameManager.Instance.level1Opened == false && GameManager.Instance.endingUnlocked == false)
        {
            return;
        }
        else
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                // Optionally store spawn position for player
                PlayerPrefs.SetFloat("SpawnX", spawnPosition.x);
                PlayerPrefs.SetFloat("SpawnY", spawnPosition.y);

                // Load the target scene
                SceneManager.LoadScene(sceneToLoad);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            interactionPanel.SetActive(true);
            if (GameManager.Instance.level1Opened == false)
            {
                interactionPrompt.text = "Level Locked!";
            }
            else
            {
                interactionPrompt.text = "Press E to enter!";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;

            interactionPrompt.text = "";
            interactionPanel.SetActive(false);
        }
    }
}

