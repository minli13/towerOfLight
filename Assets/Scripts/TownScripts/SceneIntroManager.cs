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

        DialogueUI.Instance.isCutscene = true;
        // Opening dialogue
        yield return DialogueUI.Instance.RunDialogue(openingDialogue);
        yield return DialogueUI.Instance.ShowLine("...");
       

        // Call NPCCutscene to walk to player
        NPCIntroCutscene npcCutscene = npcPatrol.GetComponent<NPCIntroCutscene>();
        npcCutscene.enabled = true; // Disable existing cutscene if any

        TownGameManager.Instance.UnlockInput();
    }
}


