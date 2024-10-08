using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public float cellSize = 1f;
    private Vector2Int lastSpawnPosition = new Vector2Int(0, 0);
    public Vector3 gridStartPosition;
    public GameObject[] jellyPrefabs;
    public GameObject fullJellyPrefab; // Declare fullJellyPrefab
    private GameObject[,] gridArray;
    public LevelConfigurator levelConfiguration;
    public ObjectiveManager objectiveManager;
    public UIManager uiManager;

    void Start()
    {
        
        if (levelConfiguration != null)
        {
            InitializeGrid(levelConfiguration.gridWidth, levelConfiguration.gridHeight);
            DisplayPlayField();
            LoadStartingJellies(); // Load starting jellies after setting up the playfield
        }
        else
        {
            Debug.LogError("LevelConfiguration is not set.");
        }
    }

    // Initialize an empty grid with specified width and height
    private void InitializeGrid(int width, int height)
    {
        gridArray = new GameObject[width, height];
    }

    // Method to calculate the world position of a grid cell based on grid coordinates
    public Vector3 GetCellWorldPosition(Vector2Int gridPosition)
    {
        float xPos = gridStartPosition.x + gridPosition.x * cellSize;
        float yPos = gridStartPosition.y;
        float zPos = gridStartPosition.z + gridPosition.y * cellSize;
        return new Vector3(xPos, yPos, zPos);
    }

    // Display the playfield by instantiating Active Cell prefabs
    private void DisplayPlayField()
    {
        for (int x = 0; x < levelConfiguration.gridWidth; x++)
        {
            for (int y = 0; y < levelConfiguration.gridHeight; y++)
            {
                Vector2Int position = new Vector2Int(x, y);

                if (levelConfiguration.blockedCells.Contains(position))
                {
                    Debug.Log($"Blocked Cell at: {position}");
                    continue;
                }

                Vector3 worldPosition = GetWorldPosition(x, y);
                GameObject activeCell = Instantiate(levelConfiguration.activeCellPrefab, worldPosition, Quaternion.identity, transform);
                activeCell.name = $"ActiveCell_{x}_{y}";
                gridArray[x, y] = activeCell;
                Debug.Log($"ActiveCell placed at: {worldPosition}");
            }
        }
    }

    // Check if a specific cell is empty
    public bool IsCellEmpty(int x, int y)
    {
        return gridArray[x, y] == null || gridArray[x, y].tag == "ActiveCell";
    }

    // Return a random jelly prefab
    public GameObject GetRandomJellyPrefab() => jellyPrefabs[Random.Range(0, jellyPrefabs.Length)];

    // Load starting jellies based on predefined positions
    private void LoadStartingJellies()
    {
        foreach (JellyInfo jellyInfo in levelConfiguration.startingJellies)
        {
            // Check if this position is blocked
            if (levelConfiguration.blockedCells.Contains(jellyInfo.position))
            {
                Debug.LogWarning($"Position {jellyInfo.position} is blocked. Skipping jelly placement.");
                continue;
            }

            GameObject jellyPrefab = GetJellyPrefabByType(jellyInfo.jellyType);
            PlaceJellyAtPosition(jellyInfo.position.x, jellyInfo.position.y, jellyPrefab);
        }
    }

     // Get jelly prefab based on type
    private GameObject GetJellyPrefabByType(JellyType jellyType)
    {
        switch (jellyType)
        {
            case JellyType.Full:
                return jellyPrefabs[0]; // Assuming this is your full jelly prefab
            case JellyType.Halves:
                return jellyPrefabs[1]; // Assuming this is your halves jelly prefab
            case JellyType.Quarters:
                return jellyPrefabs[2]; // Assuming this is your quarters jelly prefab
            default:
                Debug.LogWarning($"Unknown jelly type: {jellyType}. Defaulting to first jelly prefab.");
                return jellyPrefabs[0]; // Fallback to the first jelly prefab
        }
    }

    // Convert grid coordinates to world position
    public Vector3 GetWorldPosition(int x, int y)
    {
        float offsetX = (levelConfiguration.gridWidth - 1) / 2f * cellSize;
        float offsetY = (levelConfiguration.gridHeight - 1) / 2f * cellSize;

        float prefabHeightAdjustment = 0.5f;
        return new Vector3(x * cellSize - offsetX, prefabHeightAdjustment, y * cellSize - offsetY);
    }

    // Place a jelly at a specific grid position
    public void PlaceJellyAtPosition(int x, int y, GameObject jellyPrefab)
    {
        if (!IsWithinBounds(new Vector2Int(x, y)))
        {
            Debug.LogError($"Attempted to place jelly out of bounds at ({x}, {y})");
            return;
        }

        if (IsCellEmpty(x, y))
        {
            Vector3 spawnPosition = GetWorldPosition(x, y);
            GameObject newJelly = Instantiate(jellyPrefab, spawnPosition, Quaternion.identity);
            if (newJelly != null)
            {
                gridArray[x, y] = newJelly;
                CheckGameOver();
            }
            else
            {
                Debug.LogError("Failed to instantiate jelly prefab.");
            }
        }
        else
        {
            Debug.LogWarning($"Cell ({x}, {y}) is already occupied.");
        }
    }

    public bool IsBlockedCell(Vector2Int position) => levelConfiguration.blockedCells.Contains(position);

    public void UpdateLastSpawnPosition(Vector2Int newPosition)
    {
        lastSpawnPosition = newPosition;
    }

    public Vector2Int GetLastSpawnPosition()
    {
        return lastSpawnPosition;
    }

    public void MergeJellies(Vector2Int position)
    {
        GameObject jelly = gridArray[position.x, position.y];
        Jelly jellyComponent = jelly.GetComponent<Jelly>();

        // Define directions to check (up, down, left, right)
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var direction in directions)
        {
            Vector2Int neighborPos = position + direction;
            if (IsWithinBounds(neighborPos) && !levelConfiguration.blockedCells.Contains(neighborPos))
            {
                GameObject neighborJelly = gridArray[neighborPos.x, neighborPos.y];
                if (neighborJelly != null)
                {
                    Jelly neighborJellyComponent = neighborJelly.GetComponent<Jelly>();

                    // Check if they are the same type and color
                    if (jellyComponent.jellyColor == neighborJellyComponent.jellyColor)
                    {
                        // Combine jellies into a larger one
                        UpgradeJelly(position, neighborPos);
                        break;
                    }
                }
            }
        }
    }

    // Checks if the game is over by seeing if there are no more valid moves
    public void CheckGameOver()
    {
        for (int x = 0; x < levelConfiguration.gridWidth; x++)
        {
            for (int y = 0; y < levelConfiguration.gridHeight; y++)
            {
                if (IsCellEmpty(x, y)) return; // There are still empty cells

                // Check adjacent cells for possible merges
                Jelly currentJelly = gridArray[x, y].GetComponent<Jelly>();
                Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
                
                foreach (var dir in directions)
                {
                    Vector2Int neighborPos = new Vector2Int(x, y) + dir;
                    if (IsWithinBounds(neighborPos) && !IsBlockedCell(neighborPos))
                    {
                        if (gridArray[neighborPos.x, neighborPos.y] != null)
                        {
                            Jelly neighborJelly = gridArray[neighborPos.x, neighborPos.y].GetComponent<Jelly>();
                            if (currentJelly.jellyColor == neighborJelly.jellyColor)
                            {
                                return; // Possible merge exists
                            }
                        }
                    }
                }
            }
        }

        // If no empty cells and no possible merges
        Debug.Log("Game Over!");
        // Trigger game over UI or logic
        uiManager.ShowGameOverPanel();
    }

    // Utility method to check if a position is within grid bounds
    private bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < levelConfiguration.gridWidth &&
               position.y >= 0 && position.y < levelConfiguration.gridHeight;
    }

    // Upgrades or replaces jelly with a larger one (e.g., halves to full)
    private void UpgradeJelly(Vector2Int position, Vector2Int neighborPos)
    {
        Jelly jelly1 = gridArray[position.x, position.y].GetComponent<Jelly>();
        Jelly jelly2 = gridArray[neighborPos.x, neighborPos.y].GetComponent<Jelly>();

        objectiveManager.CollectJelly(jelly1.jellyColor);

        if (jelly1.GetType() == typeof(HalvesJelly) && jelly2.GetType() == typeof(HalvesJelly))
        {
            // Merge two halves into a full jelly
            Destroy(gridArray[position.x, position.y]);
            Destroy(gridArray[neighborPos.x, neighborPos.y]);
            GameObject newJelly = Instantiate(fullJellyPrefab, GetWorldPosition(position.x, position.y), Quaternion.identity);
            gridArray[position.x, position.y] = newJelly;
        }
    }

    // Method to return upgraded jelly type
    private GameObject GetUpgradedJellyPrefab()
    {
        // Implement the logic to return the next level jelly prefab based on game rules
        // For example, halves to full or other rules
        return fullJellyPrefab;
    }

    // Spawns a new jelly in an empty cell
    public void SpawnNewJelly()
    {
        Vector2Int spawnPosition = GetRandomEmptyCell();
        if (spawnPosition != Vector2Int.one * -1)
        {
            GameObject jellyPrefab = GetRandomJellyPrefab();
            PlaceJellyAtPosition(spawnPosition.x, spawnPosition.y, jellyPrefab);
        }
    }

    private Vector2Int GetRandomEmptyCell()
    {
        List<Vector2Int> emptyCells = new List<Vector2Int>();
        for (int x = 0; x < levelConfiguration.gridWidth; x++)
        {
            for (int y = 0; y < levelConfiguration.gridHeight; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (IsCellEmpty(x, y) && !IsBlockedCell(position))
                {
                    emptyCells.Add(position);
                }
            }
        }
        return emptyCells.Count > 0 ? emptyCells[Random.Range(0, emptyCells.Count)] : Vector2Int.one * -1;
    }

    public void IncreaseDifficulty()
    {
        // Example: Add more blocked cells
        AddRandomBlockedCell();
    }

    private void AddRandomBlockedCell()
    {
        Vector2Int randomPosition = GetRandomEmptyCell();
        if (randomPosition != Vector2Int.one * -1)
        {
            levelConfiguration.blockedCells.Add(randomPosition);
        }
    }

    // Load the level configuration and setup the grid
    public void LoadLevel(LevelConfigurator levelConfig)
    {
        levelConfiguration = levelConfig; // Set level configuration
        transform.position = levelConfig.gridStartPosition; // Set starting position
        InitializeGrid(levelConfig.gridWidth, levelConfig.gridHeight); // Initialize grid
        levelConfiguration.SetupActiveCell(transform); // Setup Active Cell prefab
    }

    private void Update()
    {
        // Example debug input for testing jelly placement
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceJellyAtPosition(0, 0, jellyPrefabs[0]); // Example placement at (0, 0)
        }
    }

    void OnDrawGizmos()
    {
        if (gridArray == null) return;

        Gizmos.color = Color.gray;
        for (int x = 0; x < levelConfiguration.gridWidth; x++)
        {
            for (int y = 0; y < levelConfiguration.gridHeight; y++)
            {
                Vector3 pos = GetWorldPosition(x, y);
                Gizmos.DrawWireCube(pos, new Vector3(cellSize, 0.1f, cellSize)); // Adjust height to match your grid
            }
        }

        // Draw the positions of the jellies for debugging
        foreach (var jelly in gridArray)
        {
            if (jelly != null)
            {
                Gizmos.color = Color.blue; // Different color for jellies
                Gizmos.DrawSphere(jelly.transform.position, 0.2f); // Adjust size for visibility
            }
        }
    }
}
