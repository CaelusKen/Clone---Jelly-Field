using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Jelly : MonoBehaviour
{
    public enum JellyType { Full, Half, Quarter }
    
    public enum JellyColor // Enum representing jelly colors
    {
        Red,
        Blue,
        Green,
        Yellow,
        Orange,
        Purple,
        Pink
    }
    
    public JellyColor jellyColor; // Store the current jelly color

    // Define an abstract method to handle merging, to be implemented by subclasses
    public abstract void MergeWith(Jelly otherJelly);
    // Define a method to specify merge behavior
    public abstract bool CanMergeWith(Jelly otherJelly);

    // Set the color of the jelly based on the JellyColor enum
    public void SetColor(JellyColor color)
    {
        jellyColor = color; // Set the jelly color to the provided enum value
        Renderer renderer = GetComponent<Renderer>();
        
        // Set the color of the material based on the JellyColor
        if (renderer != null)
        {
            switch (jellyColor)
            {
                case JellyColor.Red:
                    renderer.material.color = Color.red;
                    break;
                case JellyColor.Green:
                    renderer.material.color = Color.green;
                    break;
                case JellyColor.Blue:
                    renderer.material.color = Color.blue;
                    break;
                case JellyColor.Yellow:
                    renderer.material.color = Color.yellow;
                    break;
                case JellyColor.Orange:
                    renderer.material.color = new Color(1.0f, 0.5f, 0.0f); // Orange
                    break;
                case JellyColor.Purple:
                    renderer.material.color = new Color(0.5f, 0.0f, 0.5f); // Purple
                    break;
                case JellyColor.Pink:
                    renderer.material.color = new Color(1.0f, 0.75f, 0.8f); // Pink
                    break;
            }
        }
    }

    public Color GetUnityColor(JellyColor color)
    {
        switch (color)
        {
            case JellyColor.Red:
                return Color.red;
            case JellyColor.Green:
                return Color.green;
            case JellyColor.Blue:
                return Color.blue;
            case JellyColor.Yellow:
                return Color.yellow;
            case JellyColor.Orange:
                return new Color(1.0f, 0.5f, 0.0f); // Orange
            case JellyColor.Purple:
                return new Color(0.5f, 0.0f, 0.5f); // Purple
            case JellyColor.Pink:
                return new Color(1.0f, 0.75f, 0.8f); // Pink
            default:
                return Color.white; // Default color
        }
    }

    // Generate a random jelly color from the enum
    public void GenerateRandomColor()
    {
        // Get a random value from the JellyColor enum
        jellyColor = (JellyColor)Random.Range(0, System.Enum.GetValues(typeof(JellyColor)).Length);
        SetColor(jellyColor); // Set the jelly color
    }
}
