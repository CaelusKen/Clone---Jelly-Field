using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] jellyPrefabs;
    public Transform spawnArea;
    private GameObject currentJelly;
    private bool isJellyActive = false;

    private GridManager gridManager;

    void Start()
    {
        SpawnJelly();
    }

    void SpawnJelly()
    {
        if (currentJelly == null) 
        {
            currentJelly = Instantiate(jellyPrefabs[Random.Range(0, jellyPrefabs.Length)], GetRandomPosition(), Quaternion.identity);
            currentJelly.transform.parent = spawnArea;

            if (!currentJelly.GetComponent<JellyDrag>())
            {
                currentJelly.AddComponent<JellyDrag>();
            }

            if (!currentJelly.GetComponent<Collider>())
            {
                currentJelly.AddComponent<BoxCollider>();
            }

            isJellyActive = true;
        }
        else
        {
            // Spawn the new jelly at the last position after a successful placement
            Vector2Int lastPosition = gridManager.GetLastSpawnPosition();
            Vector3 spawnWorldPosition = gridManager.GetWorldPosition(lastPosition.x, lastPosition.y);
            currentJelly.transform.position = spawnWorldPosition;  // Move existing jelly to last spawn position
        }
    }

    // Called when a jelly is placed successfully
    public void OnJellyPlaced()
    {
        isJellyActive = false;
        currentJelly = null;
        SpawnJelly();
    }

    // Generate a random position within the spawn area
    Vector3 GetRandomPosition()
    {
        
        Vector3 spawnPosition = spawnArea.position + Vector3.up * 0.15f;
        spawnPosition.z = spawnArea.position.z;
        return spawnPosition;
    }
}
