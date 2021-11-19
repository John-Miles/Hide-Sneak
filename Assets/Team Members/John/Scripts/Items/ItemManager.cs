using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using John;
using Unity.VisualScripting;
using Random = UnityEngine.Random;


public class ItemManager : NetworkBehaviour
{
   [SerializeField] private GameManager gm;
   //LISTS
   public static List<Transform> availableSpawns = new List<Transform>();
   public List<ItemBase> items;
   [SyncVar]public List<GameObject> collectedItems;
   [SyncVar]public List<GameObject> requiredItems;
   
   //LOCATION FOR STORING COLLECTED ITEMS
   [SerializeField] private Transform collectionPoint;
   //VARIABLES FOR ITEM SPAWNING  
   [SyncVar]public int itemCount;
   [SyncVar]public int requiredCount;
   [SyncVar] int nextItem;
   [SyncVar] int nextLocation;
   //LOCATION TO SPAWN ALERT SYSTEM PREFAB
   [SyncVar] public Transform alertLocation;
   [SerializeField] private GameObject alertPrefab;
   private int nextIndex = 0;
  
   [Command(requiresAuthority = false)]
   public void CmdCountdownOutline()
   {
      RpcCoutndownOutline();
   }
   
   [ClientRpc]
   public void RpcCoutndownOutline()
   {
      StartCoroutine(DisplayItems());
   }
   
   public IEnumerator DisplayItems()
   {
      for (int i = 0; i < requiredItems.Count; i++)
      {
         requiredItems[i].GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;
      }
      yield return new WaitForSeconds(gm.preMatchCountdown);
      for (int i = 0; i < requiredItems.Count; i++)
      {
         requiredItems[i].GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
      }
      
   }

   public static void AddSpawnPoint(Transform transform)
   {
      availableSpawns.Add(transform);
   }
   public static void RemoveSpawnPoint(Transform transform) => availableSpawns.Remove(transform);

   public override void OnStartServer()
   {
      SpawnItems();
      
   }

   //[ServerCallback]
   //private void OnDestroy() => NetworkManagerHnS.OnItemReady -= SpawnItems;

   [ServerCallback]
   public void SpawnItems()
   {
      itemCount = FindObjectOfType<NetworkGamePlayerHnS>().count;
      requiredCount = FindObjectOfType<NetworkGamePlayerHnS>().itemRequired;
      for (float i = .5f; i < itemCount; i++)
      {
         nextItem = Random.Range(0, items.Count);
         nextLocation = Random.Range(0, availableSpawns.Count);
         Transform spawnPoint = availableSpawns.ElementAtOrDefault(nextLocation);

         if (spawnPoint == null)
         {
            Debug.Log($"Missing spawn point for items at {nextLocation}");
            return;
         }

         GameObject itemInstance = (items[nextItem].ItemPrefab);
         var itemSpawn = Instantiate(itemInstance, availableSpawns[nextLocation].position,
            items[nextItem].ItemPrefab.transform.rotation);
         NetworkServer.Spawn(itemSpawn);
         
         //items.RemoveAt(nextItem);
         availableSpawns.RemoveAt(nextLocation);
      }
   }

   [Command(requiresAuthority = false)]
   public void CmdRequiredFill()
   {
     RpcRequiredFill();
   }

   [ClientRpc]
   public void RpcRequiredFill()
   {
      requiredItems.Clear();
      Outline[] outlines = FindObjectsOfType<Outline>();
      foreach (var outliner in outlines)
      {
         requiredItems.Add(outliner.gameObject);
      }
   }

   [ClientRpc] public void RpcSetItemTracker()
   {
      foreach (var thief in gm.thievesInScene)
      {
         thief.GetComponent<ThiefUI>().ItemUpdate(collectedItems.Count,requiredItems.Count);
      }
   }
   
   [ClientRpc]
   public void RpcItemUpdate(GameObject item)
   {
      Debug.Log(item.name);
      collectedItems.Add(item.gameObject);
      ItemRemove(item.gameObject);
      foreach (var thief in gm.thievesInScene)
      {
         thief.GetComponent<ThiefUI>().ItemUpdate(collectedItems.Count,requiredItems.Count);
      }
      if (collectedItems.Count == requiredItems.Count)
      {
         CmdAllowEscape();
      }
      else
      {
         foreach (var guard in gm.guardsInScene)
         {
            StartCoroutine(guard.GetComponent<GuardUI>().ItemUpdate(collectedItems.Count));
         }
      }
      
      
      //check if the list of collected items matches the required items
      //if true, enable escape
      
      
   }
   
   [Command(requiresAuthority = false)]
   void CmdAllowEscape()
   {
      gm.RpcAllowEscape();
   }
   
   [ServerCallback]
   public void ItemRemove(GameObject item)
   {
      alertLocation = item.transform;
      GameObject alertInstance = Instantiate(alertPrefab, alertLocation.position, alertLocation.rotation);
      NetworkServer.Spawn(alertInstance);
      item.transform.position = collectionPoint.transform.position;
   }
}
