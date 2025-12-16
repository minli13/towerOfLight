using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private string firstScene = "Boot"; // Boot scene with player and systems


    public void StartGame()
    {
        SceneManager.LoadScene(firstScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SettingsScene()
    {
        SceneManager.LoadScene("Settings");
    }
}

