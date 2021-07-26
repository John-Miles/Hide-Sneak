using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfDetection))]
public class FieldOfDetectionEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfDetection fod = (FieldOfDetection) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fod.transform.position, Vector3.up, Vector3.forward, 360, fod.radius);


        Vector3 viewAngle1 = DirectionFromAngle(fod.transform.eulerAngles.y, -fod.angle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(fod.transform.eulerAngles.y, fod.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fod.transform.position, fod.transform.position + viewAngle1 * fod.radius);
        Handles.DrawLine(fod.transform.position, fod.transform.position + viewAngle2 * fod.radius);

        foreach (GameObject thief in fod.thiefRef)
        {
            {
                
                    Handles.color = Color.green;
                    Handles.DrawLine(fod.transform.position, thief.transform.position);
                
            }
        }

        Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}