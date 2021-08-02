using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemAlert))]
public class LightsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        ItemAlert alert = (ItemAlert)target;
        if(GUILayout.Button("Test Alert"))
        {
            alert.AlertTest();
        }
        //EditorGUILayout.LabelField("Adding custom stuff is easy mate.");
    }
}
