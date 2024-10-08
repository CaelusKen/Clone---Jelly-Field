using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Jelly;

[CreateAssetMenu(fileName = "LevelConfigurator", menuName = "ScriptableObjects/LevelConfigurator", order = 1)]
public class LevelConfigurator : ScriptableObject
{
    public Vector3 gridStartPosition; // Starting position of the grid
    public int gridWidth = 6; // Width of the grid
    public int gridHeight = 6; // Height of the grid
    public List<Vector2Int> blockedCells; // Cells that should not be occupied
    public List<Vector2Int> startingJellyPositions; // Starting positions for jellies
    public GameObject[] jellyPrefabs; // Jelly prefabs to use
    public GameObject activeCellPrefab; // Active cell prefab
    public Vector3 spawnCellPosition; // Position for spawning jellies

    public List<JellyGoal> jellyGoals = new List<JellyGoal>(); // List of jelly goals

    // Setup the Active Cell prefab in the specified parent
    public void SetupActiveCell(Transform parent)
    {
        GameObject activeCell = Instantiate(activeCellPrefab, spawnCellPosition, Quaternion.identity, parent);
        activeCell.name = "ActiveCell";
    }
}

[System.Serializable]
public class JellyGoal
{
    public JellyColor jellyColor; // The color of the jelly
    public int requiredCount; // The number of jellies required
}
