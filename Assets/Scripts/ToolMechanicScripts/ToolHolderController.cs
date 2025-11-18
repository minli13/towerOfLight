using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolHolderController : MonoBehaviour
{
    public SpriteRenderer toolRenderer;
    public SpriteRenderer playerRenderer;
    public PlayerAnimation playerAnim; // reference player animation script
    public Image selectionHighlight;

    // Offsets
    public Vector2 upOffset;
    public Vector2 downOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;

    void Update()
    {
        // Only update tool position if we have tools and one is equipped
        if (ToolRingManager.Instance != null)
        {
            UpdateToolPosition();
        }
        else
        {
            // Hide tool if no tools available
            if (toolRenderer != null)
                toolRenderer.enabled = false;
        }
    }

    void UpdateToolPosition()
    {
        int d = playerAnim.lastDirection;

        // Determine direction group
        bool isUp = (d == 0 || d == 1 || d == 7);
        bool isDown = (d == 4 || d == 3 || d == 5);
        bool isLeft = (d == 2 || d == 1 || d == 3);
        bool isRight = (d == 6 || d == 7 || d == 5);

        // Update position + sorting
        if (isUp)
        {
            transform.localPosition = upOffset;
            toolRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
            toolRenderer.flipX = false;
        }
        else if (isDown)
        {
            transform.localPosition = downOffset;
            toolRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
            toolRenderer.flipX = true; // flip for down direction
        }
        else if (isLeft)
        {
            transform.localPosition = leftOffset;
            toolRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
            toolRenderer.flipX = true;
        }
        else if (isRight)
        {
            transform.localPosition = rightOffset;
            toolRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
            toolRenderer.flipX = false;
        }
    }
}


