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
        CmdSpawnGuard();
    }

    public void ThiefPlayer()
    {
        //TODO add players network connection
        Debug.Log("prefab: " + thiefPrefab.name);
        CmdSpawnThief();
    }

    [Command]
    private void CmdSpawnThief()
    {
        GameObject newGO = Instantiate(thiefPrefab, transform.position, Quaternion.identity);
        newGO.transform.parent = transform;
        NetworkServer.Spawn(newGO, this.gameObject);
        roleCanvas.SetActive(false);
    }
    
    [Command]
    private void CmdSpawnGuard()
    {
        GameObject newGO = Instantiate(guardPrefab, transform.position, Quaternion.identity);
        newGO.transform.parent = transform;
        NetworkServer.Spawn(newGO, this.gameObject);
        roleCanvas.SetActive(false);
    }
}