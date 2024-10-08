using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    private float difficultyTimer = 0f;
    private float difficultyIncreaseInterval = 30f;
    private UIManager uiManager;

    public GridManager gridManager;

    public void UpdateScore(int points)
    {
        score += points;
        uiManager.UpdateScoreText(score); // Assumes you have a reference to UIManager
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= difficultyIncreaseInterval)
        {
            gridManager.IncreaseDifficulty();
            difficultyTimer = 0f;
        }
    }
}
