using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class FieldOfDetection : NetworkBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;

    public List<GameObject> thiefRef = new List<GameObject>();

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;


    void OnEnable()
    {
        thiefRef.Add(GameObject.FindGameObjectWithTag("Thief"));
        StartCoroutine(FODRoutine());
    }

    IEnumerator FODRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfDetectionCheck();
        }
    }

    void FieldOfDetectionCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            foreach (var t in rangeChecks)
            {
                Transform target = t.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        canSeePlayer = true;
                    }
                    else
                    {
                        canSeePlayer = false;
                    }
                }
                else
                {
                    canSeePlayer = false;
                }
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}