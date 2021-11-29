using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class animStateController : NetworkAnimator
{
    private Animator animator;

    private float velocityZ = 0.0f;

    private float velocityX = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = false;
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backwardPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);

        if (forwardPressed)
        {
            velocityZ = 1;
            isMoving = true;
        }
        if (backwardPressed)
        {
            velocityZ = -1;
            isMoving = true;
        }
        if (leftPressed)
        {
            velocityX = -1;
            isMoving = true;
        }
        if (rightPressed)
        {
            velocityX = 1;
            isMoving = true;
        }

        if ( isMoving == false)
        {
            velocityX = 0;
            velocityZ = 0;
        }
        animator.SetFloat("VelZ", velocityZ);
        animator.SetFloat("VelX", velocityX);
    }
}
