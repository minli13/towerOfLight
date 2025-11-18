using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGameManager : MonoBehaviour
{
    public static TownGameManager Instance;
    public NPCPatrol npcPatrol;

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
       
        if (npcPatrol != null)
        {
            npcPatrol.enabled = false;
        }
        // Debug.Log("Input controller script has been disabled.");
    }

    // Call this method to unlock input by enabling the script.
    public void UnlockInput()
    {
       
        if (npcPatrol != null)
        {
            npcPatrol.enabled = true;
        }
        // Debug.Log("Input controller script has been enabled.");
    }
}

