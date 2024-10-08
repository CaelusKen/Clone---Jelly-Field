using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public LevelConfigurator levelConfigurator; // Reference to LevelConfigurator
    public ObjectiveUIManager objectiveUIManager;
    private Dictionary<Jelly.JellyColor, int> jellyGoals = new Dictionary<Jelly.JellyColor, int>();
    private Dictionary<Jelly.JellyColor, int> jellyCounts = new Dictionary<Jelly.JellyColor, int>();

    void Start()
    {
        // Initialize jelly goals from LevelConfigurator
        InitializeJellyGoals();

        // Initialize jelly counts
        foreach (var goal in jellyGoals)
        {
            jellyCounts[goal.Key] = 0;
        }
    }

    private void InitializeJellyGoals()
    {
        // Clear existing goals
        jellyGoals.Clear();

        // Populate jelly goals from the level configurator
        foreach (var jellyGoal in levelConfigurator.jellyGoals)
        {
            jellyGoals[jellyGoal.jellyColor] = jellyGoal.requiredCount;
        }
    }

    public void CollectJelly(Jelly.JellyColor color)
    {
        if (jellyGoals.ContainsKey(color))
        {
            jellyCounts[color]++;
            UpdateJellyCountUI(); // Update UI on collection
            CheckObjectives();
        }

        objectiveUIManager.UpdateObjective(color);
    }

    void CheckObjectives()
    {
        bool allObjectivesMet = true;

        foreach (var goal in jellyGoals)
        {
            if (jellyCounts[goal.Key] < goal.Value)
            {
                allObjectivesMet = false;
                break;
            }
        }

        if (allObjectivesMet)
        {
            Debug.Log("All objectives met!");
            // Implement win logic here
        }
    }

    // Optional: Method to update UI with current jelly counts
    public void UpdateJellyCountUI()
    {
        foreach (var kvp in jellyCounts)
        {
            // Assuming you have a UI element for each jelly color
            // Example: jellyUIElements[kvp.Key].text = $"{kvp.Key}: {kvp.Value}/{jellyGoals[kvp.Key]}";
        }
    }
}
