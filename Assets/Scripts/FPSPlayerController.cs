using Mirror;
using UnityEngine;


public class FPSPlayerController : NetworkBehaviour
{
   [SerializeField] private float movementSpeed;
   [SerializeField] private Rigidbody rb = null;
   public bool canMove;

   public override void OnStartAuthority()
   {
      enabled = true;
      canMove = false;
   }
   
   [ClientCallback]
   private void Update() => Move();

   [Client]
   private void Move()
   {
      if (canMove)
      {
         Vector3 dir = new Vector3(0, 0, 0);
               dir.x = Input.GetAxis("Horizontal");
               dir.z = Input.GetAxis("Vertical");
               rb.transform.Translate(dir.normalized * movementSpeed * Time.deltaTime);
      }
      
   }
}

