using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Generator2D))]
public class Generator2DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Generator2D generator2D = (Generator2D)target;

        base.OnInspectorGUI();

        if (GUILayout.Button("Generate"))
        {
            generator2D.Generate();
        }

        if (GUILayout.Button("Clear"))
        {
            generator2D.Clear();
        }
    }
}
