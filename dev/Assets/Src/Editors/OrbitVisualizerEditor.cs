using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OrbitVisualizer))]
public class OrbitVisualizerEditor : Editor
{
    SerializedProperty draw_orbit;
    SerializedProperty time_into_future;
    SerializedProperty target;

    void OnEnable()
    {
        draw_orbit = serializedObject.FindProperty("draw_orbit");
        time_into_future = serializedObject.FindProperty("time_into_future");
        target = serializedObject.FindProperty("target");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(draw_orbit);
        EditorGUILayout.PropertyField(time_into_future);
        EditorGUILayout.PropertyField(target);
        serializedObject.ApplyModifiedProperties();
        if (GUILayout.Button("Draw Orbit"))
        {
            Debug.Log(target);
        }
    }

}
