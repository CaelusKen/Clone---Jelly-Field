using UnityEngine;

public class ActiveCell : MonoBehaviour
{
    public GridManager gridManager; // Reference to the GridManager
    public Vector2Int gridPosition; // Position in the grid

    private void OnMouseDown()
    {
        // Check if the grid manager is set and if the cell is empty
        if (gridManager != null && gridManager.IsCellEmpty(gridPosition.x, gridPosition.y) && !gridManager.IsBlockedCell(gridPosition))
        {
            GameObject jellyPrefab = gridManager.GetRandomJellyPrefab();
            gridManager.PlaceJellyAtPosition(gridPosition.x, gridPosition.y, jellyPrefab);
            gridManager.MergeJellies(gridPosition);
        }
    }
}
