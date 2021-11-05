using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePointRing : MonoBehaviour
{
    //References
    public GameObject escPoint, system;

    //Variables
    public float escRadius, sysRadius;

    void Awake()
    {
        escRadius = gameObject.GetComponent<SphereCollider>().radius;
        ParticleSystem.ShapeModule sMod = system.GetComponent<ParticleSystem>().shape;
        sMod.radius = escRadius;
        sysRadius = sMod.radius;
    }
}
