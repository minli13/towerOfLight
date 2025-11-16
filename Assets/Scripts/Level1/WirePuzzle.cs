using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
// Manages the wire puzzle logic
public class WirePuzzle : MonoBehaviour
{
    public LineRenderer linePrefab;
    private LineRenderer currentLine;
    private Transform startPoint;
    private List<string> completedConnections = new List<string>();
    private int connectionsNeeded = 3; // Total connections needed to complete the puzzle: blue, red, yellow

    public PuzzleUIManager puzzleUI;
    public ConduitPuzzle conduitPuzzle;

    public Camera puzzleCamera;

    void Start()
    {
        if (puzzleCamera == null)
        {
            puzzleCamera = Camera.main;
        }
    }

    void Update()
    {
        if (puzzleUI.IsPuzzleActive())
        {
            HandleInput();
        }
    }
    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector2 mousePos = puzzleCamera.ScreenToWorldPoint(Input.mousePosition);
            int layerMask = LayerMask.GetMask("WirePuzzle");
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                Debug.Log("Hit object: " + hit.collider.name + " Tag: " + hit.collider.tag);
            }
            if (hit.collider != null)
            {
                // Start a wire
                if (hit.collider.CompareTag("WireStart"))
                {
                    Debug.Log("Started wire from " + hit.collider.name);
                    startPoint = hit.collider.transform; // Store the start point
                    currentLine = Instantiate(linePrefab, transform); // Create a new line
                    currentLine.positionCount = 2; // Set two points for the line
                    currentLine.SetPosition(0, startPoint.position); // Set start position
                    currentLine.useWorldSpace = true; // Use world space for accurate positioning
                    // Set line color based on the wire color
                    if (hit.collider.name.Contains("Red"))
                        currentLine.startColor = Color.red;
                    else if (hit.collider.name.Contains("Yellow"))
                        currentLine.startColor = Color.yellow;
                    else if (hit.collider.name.Contains("Blue"))
                        currentLine.startColor = Color.blue;

                }
                // End a wire
                else if (hit.collider.CompareTag("WireEnd") && startPoint != null) // Ensure we have a start point
                {
                    if (hit.collider.name.Replace("End", "") == startPoint.name.Replace("Start", "")) // Check for correct color connection
                    {
                        currentLine.SetPosition(1, hit.collider.transform.position); // Set end position
                        StartCoroutine(FlashLine(currentLine));
                        completedConnections.Add(startPoint.name); // Mark this connection as completed
                        Debug.Log("Completed connection: " + startPoint.name);
                        hit.collider.enabled = false; // Disable the end point to prevent re-connection
                        startPoint.GetComponent<Collider2D>().enabled = false;
                        startPoint = null; // Reset start point
                        currentLine = null; // Reset current line

                        Debug.Log("Total completed connections: " + completedConnections.Count);
                        Debug.Log("Connections needed: " + connectionsNeeded);
                        Debug.Log("Is puzzle completed? " + (completedConnections.Count >= connectionsNeeded));
                        // Check for puzzle completion
                        if (completedConnections.Count >= connectionsNeeded) 
                        {
                            puzzleUI.HidePuzzle();
                            conduitPuzzle.OnPuzzleCompleted();
                            Debug.Log("Wire puzzle completed!");
                        }
                    }
                    else
                    {
                        Debug.Log("Wrong connection!");
                        Destroy(currentLine.gameObject); // Remove the incorrect line if wrong connection
                        startPoint = null; // Reset start point
                    }
                }
            }
        }
        // Update current line position while dragging
        if (currentLine != null)
        {
            Vector3 mousePos = puzzleCamera.ScreenToWorldPoint(Input.mousePosition); // Get mouse position in world space
            mousePos.z = 0; // Set z to 0 for 2D
            currentLine.SetPosition(1, mousePos); // Update end position to mouse
        }
    }

    private IEnumerator SmoothSnap(LineRenderer line, Vector3 targetPosition, float duration = 0.15f)
    {
        Vector3 startPos = line.GetPosition(1);
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            Vector3 newPos = Vector3.Lerp(startPos, targetPosition, time / duration);
            line.SetPosition(1, newPos);
            yield return null;
        }

        line.SetPosition(1, targetPosition);
    }

    private IEnumerator FlashLine(LineRenderer line, float duration = 0.1f)
    {
        Color original = line.startColor;
        line.endColor = original;
        yield return new WaitForSeconds(duration);
        line.startColor = original;
        line.endColor = original;
    }

}


