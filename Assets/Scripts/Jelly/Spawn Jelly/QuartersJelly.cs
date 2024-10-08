using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuartersJelly : Jelly
{
    public GameObject[] quarters; // Array of quarter jelly parts

    void Start()
    {
        SetQuartersColors(); // Set different random colors for each quarter
    }

    // Set random color for each quarter
    public void SetQuartersColors()
    {
        foreach (GameObject part in quarters)
        {
            JellyColor randomColor = (JellyColor)Random.Range(0, System.Enum.GetValues(typeof(JellyColor)).Length);
            Color unityColor = GetUnityColor(randomColor); // Convert to Unity Color
            Renderer renderer = part.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = unityColor; // Apply the color
            }
        }
    }

    public override bool CanMergeWith(Jelly otherJelly)
    {
        return otherJelly is HalvesJelly;
    }

    public override void MergeWith(Jelly otherJelly)
    {
        if (otherJelly is HalvesJelly)
        {
            Destroy(gameObject); // Destroy this quarter jelly
        }
    }
}
