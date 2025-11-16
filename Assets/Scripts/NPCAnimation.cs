using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    private Animator anim;
    public string[] staticDirections = { "Static N", "Static NW", "Static W", "Static SW", "Static S", "Static SE", "Static E", "Static NE" };
    public string[] runDirections = { "Run N", "Run NW", "Run W", "Run SW", "Run S", "Run SE", "Run E", "Run NE" };
    public int lastDirection;


    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null) Debug.LogError($"{name}: Animator NOT FOUND!");
    }


    public void SetDirection(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.01f)
        {
            anim.Play(staticDirections[lastDirection]); // idle
            return;
        }

        int directionIndex = DirectionToIndex(dir);
        lastDirection = directionIndex;
        anim.Play(runDirections[directionIndex]);
    }


    /// Converts a Vector2 direction into an 8-direction index
    int DirectionToIndex(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        if (angle < 22.5f) return 6;   // E
        if (angle < 67.5f) return 7;   // NE
        if (angle < 112.5f) return 0;  // N
        if (angle < 157.5f) return 1;  // NW
        if (angle < 202.5f) return 2;  // W
        if (angle < 247.5f) return 3;  // SW
        if (angle < 292.5f) return 4;  // S
        if (angle < 337.5f) return 5;  // SE
        return 6; // fallback
    }
}
