using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(GameManager))]
public class GameEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Start The Game"))
        {
            (target as GameManager)?.RpcPlayerListUpdate();
        }
        
        if (GUILayout.Button("Call Time Up"))
        {
            (target as GameManager)?.RPCTimeExpired();
        }

        if (GUILayout.Button("Call Thief Escaped"))
        {
            (target as GameManager)?.AllEscaped();
        }

        if (GUILayout.Button("Call Thief Caught"))
        {
            (target as GameManager)?.AllCaught();
        }

        if (GUILayout.Button("Allow Escape"))
        {
            (target as GameManager)?.AllowEscape();
        }
        
       
    }
}
