using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullJelly : Jelly
{
    void Start()
    {
        GenerateRandomColor(); // Set a random color at startup
    }

    public override bool CanMergeWith(Jelly otherJelly)
    {
        return otherJelly is HalvesJelly;
    }

    public override void MergeWith(Jelly otherJelly)
    {
        // Check if the other jelly is a HalfJelly
        if (otherJelly is HalvesJelly)
        {
            // Define logic to combine with halves, e.g., destroy halves and create a full jelly
            Destroy(otherJelly.gameObject); // Destroy the half jelly
            // Additional logic for merging, if needed
        }
    }
}
