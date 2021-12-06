using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class animStateControllerCopy : NetworkAnimator
{
    [SyncVar]  float velocityZ = 0.0f;

    [SyncVar] float velocityX = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
   
    // Update is called once per frame
    void Update()
    {
        MovementVelocitys();
    }

    
    void MovementVelocitys()
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

            if (isMoving == false)
            {
                velocityX = 0;
                velocityZ = 0;
            }

            animator.SetFloat("VelZ", velocityZ);
            animator.SetFloat("VelX", velocityX);
     
    }
}