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


  private void OnEnable()
  {
    if (isLocalPlayer)
    {
      roleCanvas.SetActive(true);
    }
  }
  
  public void GuardPlayer()
  {
    //TODO add players network connection
    Debug.Log("prefab: " + guardPrefab.name);
    CmdSpawnPlayer(guardPrefab);
  }

  public void ThiefPlayer()
  {
    //TODO add players network connection
    Debug.Log("prefab: " + thiefPrefab.name);
      CmdSpawnPlayer(thiefPrefab);
  }
  
  [Command]
  private void CmdSpawnPlayer(GameObject player)
  {
    GameObject newGO = Instantiate(player, transform.position, Quaternion.identity);
      NetworkServer.Spawn(newGO, this.gameObject);
      roleCanvas.SetActive(false);
      
      
  }
}
