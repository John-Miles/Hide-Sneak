using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePoint : MonoBehaviour
{
    public GameManager gameManager;

    public void OnEnable()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Thief"))
        {
            gameManager.ThiefEscaped(other.gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 1f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position + transform.forward * 2);
    }
}
