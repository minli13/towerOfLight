using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;        // Waypoints to follow
    public float speed = 2f;
    public float reachThreshold = 0.1f;
    public float waitTimeAtWaypoint = 1f;

    private int currentWP = 0;
    private float waitTimer = 0f;

    private NPCAnimation animController;

    void Start()
    {
        if (TownGameManager.Instance != null)
        {
            TownGameManager.Instance.LockInput();
        }

        animController = GetComponentInChildren<NPCAnimation>();
        if (animController == null)
            Debug.LogError($"{name}: NPCAnimation component not found!");

        if (waypoints.Length == 0)
            Debug.LogWarning($"{name}: No waypoints assigned!");
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWP];

        // Waiting at waypoint
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            animController?.SetDirection(Vector2.zero);
            return;
        }

        // Movement direction
        Vector3 moveDir = (target.position - transform.position);
        float distance = moveDir.magnitude;
        Vector2 moveDirNormalized = moveDir.normalized;

        // Move NPC
        transform.position += (Vector3)moveDirNormalized * speed * Time.deltaTime;
        animController?.SetDirection(moveDirNormalized);

        // Reached waypoint?
        if (distance < reachThreshold)
        {
            // If last waypoint, STOP
            if (currentWP == waypoints.Length - 2)
            {
                StopMovement(); // stops anim + movement


                return;
            }

            // Otherwise, continue like normal
            currentWP++;
            waitTimer = waitTimeAtWaypoint;
        }
    }


    void StopMovement()
    {
        animController?.SetDirection(Vector2.zero);   // idle
        enabled = false;                              // turn off this script completely
    }

}


