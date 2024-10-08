using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] jellyPrefabs;
    public Transform spawnArea;
    public int spawnCount = 5;

    void Start()
    {
        SpawnJellies();
    }

    void SpawnJellies()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject jelly = Instantiate(jellyPrefabs[Random.Range(0, jellyPrefabs.Length)], GetRandomPosition(), Quaternion.identity);
            jelly.transform.parent = spawnArea;
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-spawnArea.localScale.x / 2, spawnArea.localScale.x / 2);
        float y = Random.Range(-spawnArea.localScale.y / 2, spawnArea.localScale.y / 2);
        Vector3 spawnPosition = spawnArea.position + new Vector3(x, y, 0);
        
        return spawnPosition;
    }
}
