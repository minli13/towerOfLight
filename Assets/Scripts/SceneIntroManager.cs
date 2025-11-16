using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class SceneIntroManager : MonoBehaviour
{
    public DialogueSequence openingDialogue;
    public DialogueSequence npcDialogue;
    public NPCPatrol npcPatrol;
    public Transform[] introWaypoints;

    private void Start()
    {
        StartCoroutine(RunIntro());
    }

    private IEnumerator RunIntro()
    {
        Debug.Log("Starting intro cutscene.");
        // Lock player + NPC input during intro
        // TownGameManager.Instance.LockInput();

        DialogueUI.Instance.isCutscene = true;
        // Opening dialogue
        yield return DialogueUI.Instance.RunDialogue(openingDialogue);
        yield return DialogueUI.Instance.ShowLine("...");
        Debug.Log("Opening dialogue finished.");

        // Call NPCCutscene to walk to player
        NPCIntroCutscene npcCutscene = npcPatrol.GetComponent<NPCIntroCutscene>();
        npcCutscene.enabled = true; // Disable existing cutscene if any

        /*
        // NPC walks toward player
        npcPatrol.waypoints = introWaypoints;
        npcPatrol.speed = 2f;

        // Wait until NPC reaches last waypoint
        Transform lastWP = introWaypoints[introWaypoints.Length - 1];
        while (Vector3.Distance(npcPatrol.transform.position, lastWP.position) > 0.1f)
        {
            yield return null;
        }

        // NPC talking dialogue
        yield return DialogueUI.Instance.RunDialogue(npcDialogue);
        */
        // end intro — give control back
        TownGameManager.Instance.UnlockInput();
    }
}


