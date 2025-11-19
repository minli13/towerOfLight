using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "TowerLevel1"; // target scene
    [SerializeField] private Vector2 spawnPosition; // optional spawn point in new scene
    public GameObject InteractionPanel;

    private bool playerInRange = false;

    void Update()
    {
        // Check if level is unlocked
        if (GameManager.Instance.level1Opened == false)
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
            if (GameManager.Instance.level1Opened == false)
            {
                InteractionPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Level Locked!";
                return;
            }
            else
            {
                InteractionPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Press E to enter!";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}

