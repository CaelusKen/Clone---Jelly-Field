using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelConfigurator))]
public class LevelConfigurationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelConfigurator configurator = (LevelConfigurator)target;

        // Draw the default inspector properties
        DrawDefaultInspector();

        // Display jelly goals
        EditorGUILayout.LabelField("Jelly Goals", EditorStyles.boldLabel);
        for (int i = 0; i < configurator.jellyGoals.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            configurator.jellyGoals[i].jellyColor = (Jelly.JellyColor)EditorGUILayout.EnumPopup("Jelly Color", configurator.jellyGoals[i].jellyColor);
            configurator.jellyGoals[i].requiredCount = EditorGUILayout.IntField("Required Count", configurator.jellyGoals[i].requiredCount);
            EditorGUILayout.EndHorizontal();
        }

        // Button to add a new jelly goal
        if (GUILayout.Button("Add Jelly Goal"))
        {
            configurator.jellyGoals.Add(new JellyGoal());
        }

        // Button to remove the last jelly goal
        if (GUILayout.Button("Remove Last Jelly Goal") && configurator.jellyGoals.Count > 0)
        {
            configurator.jellyGoals.RemoveAt(configurator.jellyGoals.Count - 1);
        }

        // Save changes to the scriptable object
        if (GUI.changed)
        {
            EditorUtility.SetDirty(configurator);
        }
    }
}
