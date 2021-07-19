using Mirror;
using UnityEngine;

namespace Team_Members.NidgyWidgy.Scripts
{
  public class Flashlight : NetworkBehaviour
  {

 
    public GameObject torch;
    public bool TorchOn = true;


    void Update()
    {
      if (Input.GetKeyDown(KeyCode.F))
      {
        TorchOn = !TorchOn;
        torch.SetActive(TorchOn);
      }
    
    }
  
  }
}
