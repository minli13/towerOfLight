using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnSystem : MonoBehaviour
{

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayerSpawnPoint spawn = FindObjectOfType<PlayerSpawnPoint>();
        if (spawn != null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = spawn.transform.position;
        }
    }
}
