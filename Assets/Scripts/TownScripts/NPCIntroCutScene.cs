using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCIntroCutscene : MonoBehaviour
{
    [Header("Movement")]
    public Transform walkToPlayerPoint;
    public Transform walkAwayPoint;
    public float speed = 1.5f;

    private NPCAnimation npcAnim;

    [Header("Dialogue")]
    public DialogueSequence neighborIntroCutscene;

    private void Start()
    {
        npcAnim = GetComponentInChildren<NPCAnimation>();
        // Delay start slightly to ensure UI is ready
        Invoke(nameof(StartCutscene), 0.2f);
    }

    private void StartCutscene()
    {
        // Lock player input during cutscene
        TownGameManager.Instance.LockInput();

        StartCoroutine(RunCutscene());
    }

    private IEnumerator RunCutscene()
    {
        // Walk to player
        yield return StartCoroutine(MoveToPosition(walkToPlayerPoint.position));

        // Play dialogue
        yield return DialogueUI.Instance.RunDialogue(neighborIntroCutscene);

        // Walk away
        yield return StartCoroutine(MoveToPosition(walkAwayPoint.position));

        // Unlock player input after cutscene
        TownGameManager.Instance.UnlockInput();
        GameManager.Instance.UnlockInput();
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            Vector2 direction = (target - transform.position).normalized;

            transform.position += (Vector3)(direction * speed * Time.deltaTime);

            // Update NPC animation to face movement direction
            npcAnim.SetDirection(direction);

            yield return null;
        }

        // Stop moving once reached target
        npcAnim.SetDirection(Vector2.zero);
    }
}

