using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyDrag : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private ObjectiveManager objectiveManager;
    private Vector3 originalPosition;
    private GridManager gridManager;

    void Start()
    {
        // Find the ObjectiveManager in the scene
        objectiveManager = FindObjectOfType<ObjectiveManager>();
        gridManager = FindObjectOfType<GridManager>();
        originalPosition = transform.position;
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
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        // Get the current world position of the jelly
        Vector3 droppedPosition = transform.position;

        // Convert world position to grid coordinates
        Vector2Int gridPosition = GetGridPosition(droppedPosition);

        Vector3 snappedPosition = gridManager.GetWorldPosition(gridPosition.x, gridPosition.y);
        transform.position = snappedPosition;

        // Check if the jelly is placed on a valid playfield area
        if (gridManager.IsCellEmpty(gridPosition.x, gridPosition.y) &&
            !gridManager.IsBlockedCell(gridPosition))
        {
            Jelly jellyComponent = GetComponent<Jelly>(); // Assuming this script is on the Jelly prefab
            if (jellyComponent != null)
            {
                objectiveManager.CollectJelly(jellyComponent.jellyColor);
                gridManager.PlaceJellyAtPosition(gridPosition.x, gridPosition.y, gameObject);
            }
            else
            {
                Debug.LogError("Jelly component not found.");
            }
        }
        else
        {
            Debug.Log("Invalid placement! Returning to original position.");
            transform.position = originalPosition;
        }
    }

    private Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        // Assuming the origin (0,0) of the grid is at the center of the playfield
        int x = Mathf.RoundToInt(worldPosition.x / gridManager.cellSize);
        int y = Mathf.RoundToInt(worldPosition.z / gridManager.cellSize);
        return new Vector2Int(x, y);
    }

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
