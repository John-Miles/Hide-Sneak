using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class DetectBehaviour : NetworkBehaviour
{
    public float radius;
    [Range(0, 360)] public float angle;
    
    public LayerMask thiefLayer;
    public LayerMask obstructionMask;

    public void Detect()
    {
        Collider[] inDetection = Physics.OverlapSphere(transform.position, radius, thiefLayer);

        if (inDetection.Length != 0)
        {
            for (int i = 0; i < inDetection.Length; i++)
            {
                Collider thief = inDetection[i];
                Vector3 directionToThief = (transform.position - thief.transform.position).normalized;
                float distanceToThief = Vector3.Distance(transform.position, thief.transform.position);

                if (Vector3.Angle(transform.forward, directionToThief) < angle / 2)
                {
                    RaycastHit hit;

                    Physics.Raycast(transform.position, directionToThief, out hit, obstructionMask);

                    if (hit.collider == thief)
                    {
                        thief.GetComponentInParent<ThiefStatistics>().inExpose = true;
                    }

                    if (distanceToThief > radius)
                    {
                        thief.GetComponentInParent<ThiefStatistics>().inExpose = false;
                    }
                    
                    else if (hit.collider != inDetection[i])
                    {
                        thief.GetComponentInParent<ThiefStatistics>().inExpose = false;
                    }

                }
                else
                {
                    thief.GetComponentInParent<ThiefStatistics>().inExpose = false;
                }



            }
        }
    }
    private void Update()
    {
        Detect();
    }
}
