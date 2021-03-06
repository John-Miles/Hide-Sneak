using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Jesse
{
    public class ItemRotation : MonoBehaviour
    {
        [Tooltip("The speed of the cube rotation.")]
        public float spinSpeed;

        [Tooltip("The height of the bounce (Wave Amplitude.)")]
        public float bounceHeight = 0.5f;

        [Tooltip("The amount of bounces (Wave Frequency.)")]
        public float bounceSpeed = 1f;

        Vector3 posOffset = new Vector3();
        Vector3 tempPos = new Vector3();
        
        public float spinX;
        public float spinY;
        public float spinZ;
        

        // Start is called before the first frame update
        void Start()
        {
            posOffset = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(spinX, spinY, spinZ);

            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * bounceSpeed) * bounceHeight;
                
            //transform.position = tempPos;
        }
    }

}