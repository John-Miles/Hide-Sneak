using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePoint : MonoBehaviour
{
    public GameManager gameManager;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 1f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position + transform.forward * 2);
    }
}
