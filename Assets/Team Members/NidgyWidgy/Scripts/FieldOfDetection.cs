using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class FieldOfDetection : NetworkBehaviour
{
    public float radius;
    [Range(0, 360)] public float angle;

    public List<GameObject> thiefRef = new List<GameObject>();

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;


    void OnEnable()
    {

        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Thief")) thiefRef.Add(o);
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
        thiefRef.Clear();
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);


        if (rangeChecks.Length != 0)
        {
            for (int t = 0; t < rangeChecks.Length; t++)

            {
                Collider target = rangeChecks[t];
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                    RaycastHit hit;

                    Physics.Raycast(transform.position, directionToTarget, out hit, obstructionMask);
                    if (hit.collider == rangeChecks[t])
                    {
                        thiefRef.Add(target.gameObject);
                        if (!target.GetComponentInParent<ThiefStats>().seen)
                        {
                            target.GetComponentInParent<ThiefStats>().seen = true;
                        }
                    }
                }
                else
                {
                    target.GetComponentInParent<ThiefStats>().seen = false;
                }
            }
        }
    }
}