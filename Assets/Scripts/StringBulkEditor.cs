using UnityEngine;
using UnityEditor;
using System;
using Assets.Scripts.Campaign.DND_Download;

[CustomEditor(typeof(DNDFilter))] // Replace with your component name
public class StringBulkEditor : Editor
{
    private string rawBulkInput = "";
    private enum TargetList { values, keys }
    private TargetList chosenList = TargetList.values;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Bulk String Importer", EditorStyles.boldLabel);

        // Draw a dropdown menu in the Inspector
        chosenList = (TargetList)EditorGUILayout.EnumPopup("Target List", chosenList);

        // A large text area for pasting multi-line text blocks
        EditorGUILayout.LabelField("Paste items below (One per line):");
        rawBulkInput = EditorGUILayout.TextArea(rawBulkInput, GUILayout.Height(100));

        if (GUILayout.Button("Append to List", GUILayout.Height(30)))
        {
            // Split by newlines, removing completely empty rows
            string[] lines = rawBulkInput.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length > 0)
            {  // 3. Convert the enum selection into the exact C# variable name
                string exactTargetPropertyName = "";
                switch (chosenList)
                {
                    case TargetList.values:
                        exactTargetPropertyName = "values";
                        break;
                    case TargetList.keys:
                        exactTargetPropertyName = "keys";
                        break;
                }

                // 4. Pass the dynamically selected property name
                AddStringsToSerializedList(exactTargetPropertyName, lines);

                rawBulkInput = "";
                GUI.FocusControl(null);
            }
        }
    }

    private void AddStringsToSerializedList(string propertyName, string[] newStrings)
    {
        serializedObject.Update();
        SerializedProperty listProperty = serializedObject.FindProperty(propertyName);

        if (listProperty != null && listProperty.isArray)
        {
            int oldSize = listProperty.arraySize;
            listProperty.arraySize += newStrings.Length;

            for (int i = 0; i < newStrings.Length; i++)
            {
                SerializedProperty element = listProperty.GetArrayElementAtIndex(oldSize + i);
                element.stringValue = newStrings[i].Trim();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}