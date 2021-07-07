using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Start The Game"))
        {
            (target as GameManager)?.StartRound();
        }
        
       
    }
}
