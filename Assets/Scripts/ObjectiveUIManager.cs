using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveUIManager : MonoBehaviour
{
    public TMP_Text objectivesText;
    public List<JellyGoal> jellyGoals;
    // Start is called before the first frame update
    void Start()
    {
        UpdateObjectivesDisplay();
    }

    public void UpdateObjectivesDisplay()
    {
        string objectives = "Objectives:\n";
        foreach (JellyGoal goal in jellyGoals)
        {
            objectives += $"{goal.requiredCount} {goal.jellyColor} Jelly(s)\n";
        }
        objectivesText.text = objectives;
    }

    public void UpdateObjective(Jelly.JellyColor jellyColor)
    {
        // Update logic when a jelly is collected
        foreach (JellyGoal goal in jellyGoals)
        {
            if (goal.jellyColor == jellyColor)
            {
                goal.requiredCount--;
                if (goal.requiredCount <= 0)
                {
                    // Handle completion of the objective (e.g., mark as complete)
                }
                UpdateObjectivesDisplay();
                break;
            }
        }
    }
}
