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
    private List<string> correctConnections = new List<string>();
    private List<string> wrongConnections = new List<string>();
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
                        correctConnections.Add(startPoint.name); // Mark this connection as correct
                        Debug.Log("Completed connection: " + startPoint.name);
                        hit.collider.enabled = false; // Disable the end point to prevent re-connection
                        startPoint.GetComponent<Collider2D>().enabled = false;
                        startPoint = null; // Reset start point
                        currentLine = null; // Reset current line

                    }
                    // If click on same color start part, remove current line
                    else if (hit.collider.name == startPoint.name)
                    {
                        Debug.Log("Cancelled connection from " + startPoint.name);
                        Destroy(currentLine.gameObject); // Remove the line
                        startPoint.GetComponent<Collider2D>().enabled = true; // Re-enable the start point
                        startPoint = null; // Reset start point
                        currentLine = null; // Reset current line
                    }
                    else
                    {
                        Debug.Log("Wrong connection!");
                        // Destroy(currentLine.gameObject); // Remove the incorrect line if wrong connection
                        // Count wrong connections, if 3 wrong connections, flag puzzle failed
                        
                        
                        // Destroy(currentLine.gameObject); // Remove the incorrect line if wrong connection
                        // If wrong connection, connect and add to wrong connections
                        currentLine.SetPosition(1, hit.collider.transform.position); // Set end position
                        wrongConnections.Add(startPoint.name); // Mark this connection as wrong
                        completedConnections.Add(startPoint.name); // Still count towards completed connections
                        StartCoroutine(FlashLine(currentLine, 0.5f));
                        hit.collider.enabled = false; // Disable the end point to prevent re-connection
                        startPoint = null; // Reset start point
                        currentLine = null; // Reset current line

                        if (wrongConnections.Count >= 3)
                        {
                            Debug.Log("Too many wrong connections! Puzzle failed.");
                            puzzleUI.OnPuzzleFailed();
                            // Reset puzzle state
                            completedConnections.Clear();
                            wrongConnections.Clear();
                        }

                        startPoint = null; // Reset start point
                    }
                    IsPuzzleCompleted();
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

    public bool IsPuzzleCompleted()
    {
        // Check for puzzle completion
        if (completedConnections.Count >= connectionsNeeded)
        {
            puzzleUI.HidePuzzle();
            conduitPuzzle.OnPuzzleCompleted();
            Debug.Log("Wire puzzle completed!");
            return true;
        }
        return false;
    }

    public void ResetPuzzle()
    {
        // Destroy all existing lines
        LineRenderer[] existingLines = GetComponentsInChildren<LineRenderer>();
        foreach (LineRenderer line in existingLines)
        {
            Destroy(line.gameObject);
        }
        completedConnections.Clear();
        wrongConnections.Clear();
        startPoint = null;
        currentLine = null;
        // Re-enable all wire endpoints
        GameObject[] wireEnds = GameObject.FindGameObjectsWithTag("WireEnd");
        foreach (GameObject end in wireEnds)
        {
            end.GetComponent<Collider2D>().enabled = true;
        }
        GameObject[] wireStarts = GameObject.FindGameObjectsWithTag("WireStart");
        foreach (GameObject start in wireStarts)
        {
            start.GetComponent<Collider2D>().enabled = true;
        }
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


