using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyDrag : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Vector3 originalPosition;
    private GridManager gridManager;

    private float minY; 
    
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();  // Reference to the grid manager
        originalPosition = transform.position;

        minY = 0.5f;
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPosition() + offset;  // Dragging movement

            // Restrict movement below a certain Y value
            if (newPosition.y < minY)
            {
                newPosition.y = minY; // Keep jelly at or above the minimum Y
            }

            transform.position = newPosition; // Update position
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Get the current world position of the jelly
        Vector3 droppedPosition = transform.position;

        // Convert world position to grid coordinates
        Vector2Int gridPosition = GetGridPosition(droppedPosition);

        // Snap the jelly to the grid position
        Vector3 snappedPosition = gridManager.GetWorldPosition(gridPosition.x, gridPosition.y);
        snappedPosition.y = minY; // Ensure jelly is positioned correctly above the grid

        // Place the jelly only if the grid cell is valid and empty
        if (gridManager.IsCellEmpty(gridPosition.x, gridPosition.y) && 
            !gridManager.IsBlockedCell(gridPosition))
        {
            transform.position = snappedPosition;  // Snap jelly to grid
            gridManager.PlaceJellyAtPosition(gridPosition.x, gridPosition.y, gameObject);
            
            // Update last spawn position when placing jelly
            gridManager.UpdateLastSpawnPosition(gridPosition); // This line is correct
        }
        else
        {
            Debug.Log("Invalid placement! Returning to original position.");
            transform.position = originalPosition;  // Return jelly to original position if invalid
        }
    }

    // Converts a world position into grid coordinates
    private Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        // Assuming the origin (0,0) of the grid is at the center of the playfield
        int x = Mathf.RoundToInt(worldPosition.x / gridManager.cellSize);
        int y = Mathf.RoundToInt(worldPosition.z / gridManager.cellSize);  // Assuming grid is on the XZ plane
        return new Vector2Int(x, y);
    }

    // Get the world position of the mouse
    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return transform.position;
    }
}
