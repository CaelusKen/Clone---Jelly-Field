using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalvesJelly : Jelly
{
    public GameObject[] halves; // Array of half jelly parts

    void Start()
    {
        SetHalvesColors(); // Set different random colors for each half
    }

    // Set random color for each half
    public void SetHalvesColors()
    {
        foreach (GameObject part in halves)
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
        if (otherJelly is FullJelly)
        {
            Destroy(gameObject); // Destroy this half jelly
        }
    }
}
