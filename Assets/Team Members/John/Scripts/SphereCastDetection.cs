using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEditor;
using UnityEngine;

public class SphereCastDetection : NetworkBehaviour
{
    public GameManager gm;
    public float radius;
    [Range(0, 360)] public float angle;
    public float maxDistance;
    public List<GameObject> thiefRef = new List<GameObject>();
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public Collider[] colliders;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        enabled = true;
    }
    
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Detect();
    }
    
    public void Detect()
    {
        Debug.Log("Does this work?");
        gm.CmdExposeRemove();
        thiefRef.Clear();
        
        if (gameObject.GetComponent<FlashLight>().isOn)
        {
            RaycastHit hit;
            colliders = Physics.OverlapSphere(transform.position, radius, targetMask);

            foreach (var target in colliders)
            {
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
                
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    if (Physics.SphereCast(transform.position, radius, directionToTarget, out hit, maxDistance,
                        obstructionMask))
                    {
                        thiefRef.Add(target.gameObject);
                        foreach (var thief in thiefRef)
                        {
                            gm.CmdExposeUpdate(thief);
                        }
                        Debug.Log("Found a thief!");
                    }
                }
            
            }
        }
        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position,radius);
    }
}
