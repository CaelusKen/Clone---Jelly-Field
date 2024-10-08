using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GridManager gridManager; // Reference to the GridManager
    public LevelConfigurator levelConfiguration; // Reference to the LevelConfigurator

    void Start()
    {
        LoadLevel(); // Load the level at startup
    }

    public void LoadLevel()
    {
        if (gridManager != null && levelConfiguration != null)
        {
            gridManager.LoadLevel(levelConfiguration); // Load the level in GridManager
        }
        else
        {
            Debug.LogWarning("GridManager or LevelConfiguration is not assigned!");
        }
    }
}
