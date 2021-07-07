using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class DebugRoleSelect : NetworkBehaviour
{
  public GameObject player;
  private GameObject thisUser;
  
  public void SpawnPlayer()
  {
    GameObject newGO = Instantiate(player, transform.position, Quaternion.identity);
      NetworkServer.Spawn(newGO,thisUser);
  }
}
