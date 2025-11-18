using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerMovement playerMovement;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this method to lock input by disabling the script.
    public void LockInput()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    
    }

    // Call this method to unlock input by enabling the script.
    public void UnlockInput()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

    }
}
