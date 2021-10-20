using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private void Awake() => ItemManager.AddSpawnPoint(transform);

    private void OnDestroy() => ItemManager.RemoveSpawnPoint(transform);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position + transform.forward * 2);
    }
}
