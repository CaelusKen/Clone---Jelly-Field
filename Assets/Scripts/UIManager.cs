using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;

    public GameObject gameOverPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
