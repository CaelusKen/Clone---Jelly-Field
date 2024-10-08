using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalvesJelly : Jelly
{
    public GameObject[] halves; // Array of half jelly parts

    public override bool CanMergeWith(Jelly otherJelly)
    {
        return otherJelly is HalvesJelly;
    }

    void Start()
    {
        GenerateRandomColor(); // Set a random color for halves
        SetHalfColor(jellyColor); // Set color for half jellies based on the jellyColor enum
    }

    // Set color for each half
    public void SetHalfColor(JellyColor color)
    {
        Color unityColor = GetUnityColor(color); // Convert to Unity Color
        foreach (GameObject part in halves)
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
        // Check if the other jelly is a FullJelly
        if (otherJelly is FullJelly)
        {
            // Logic for merging with a full jelly could go here
            // For example, you might want to destroy this half and the full jelly,
            // or combine them to create a larger jelly.
            Destroy(gameObject); // Destroy this half jelly
            // Add additional logic if required
        }
    }
}
