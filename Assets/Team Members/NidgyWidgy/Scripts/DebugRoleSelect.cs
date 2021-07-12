using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class DebugRoleSelect : NetworkBehaviour
{

  public GameObject roleCanvas;
  public GameObject guardPrefab;
  public GameObject thiefPrefab;

  public void GuardPlayer()
  {
    //TODO add players network connection
    SpawnPlayer(guardPrefab, null);
  }

  public void ThiefPlayer()
  {
    //TODO add players network connection
      SpawnPlayer(thiefPrefab, null);
  }
  
  
  public void SpawnPlayer(GameObject player, NetworkConnection conn)
  {
    GameObject newGO = Instantiate(player, transform.position, Quaternion.identity);
      NetworkServer.Spawn(newGO,conn);
      roleCanvas.SetActive(false);
  }
}
