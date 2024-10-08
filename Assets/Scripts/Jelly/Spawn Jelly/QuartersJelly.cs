using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuartersJelly : Jelly
{
    public GameObject[] quarters; // Array of quarter jelly parts

    public override bool CanMergeWith(Jelly otherJelly)
    {
        return otherJelly is HalvesJelly;
    }

    void Start()
    {
        GenerateRandomColor(); // Set a random color for quarters
        SetQuarterColor(jellyColor); // Set color for quarter jellies based on the jellyColor enum
    }

    // Set color for each quarter
    public void SetQuarterColor(JellyColor color)
    {
        Color unityColor = GetUnityColor(color); // Convert to Unity Color
        foreach (GameObject part in quarters)
        {
            Renderer renderer = part.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = unityColor; // Apply the color
            }
        }
    }

    public override void MergeWith(Jelly otherJelly)
    {
        // Check if the other jelly is a HalvesJelly
        if (otherJelly is HalvesJelly)
        {
            // Logic for merging with a halves jelly could go here
            Destroy(gameObject); // Destroy this quarter jelly
            // Add additional logic if required
        }
    }
}
