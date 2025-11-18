using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // starting scene

    private void Awake()
    {
        // Ensure this object persists across scene loads
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // Load the initial gameplay scene
        SceneManager.LoadScene(sceneToLoad);

        // Disable tool ring at start
        ToolRingManager.Instance.toolRingPanel.SetActive(false);
    }
}

