using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMenu : MonoBehaviour
{
    public GameObject stats;

    public void Start()
    {
        stats = GameObject.Find("StatsPanel");
        if (stats != null)
        {
            stats.SetActive(false); // Disable stats display
        }
        FlickerDarkness flicker = GetComponent<FlickerDarkness>();
        if (flicker != null)
        {
            flicker.enabled = false; // Disable flickering effect
        }

    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
