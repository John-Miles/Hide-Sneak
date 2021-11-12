using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using John;
using Random = UnityEngine.Random;


public class ItemManager : NetworkBehaviour
{
   [SerializeField] private GameManager gm;
   //LISTS
   public static List<Transform> availableSpawns = new List<Transform>();
   public List<John.ItemBase> items;
   [SyncVar]public List<GameObject> collectedItems;
   [SyncVar]public List<GameObject> requiredItems;
   
   //LOCATION FOR STORING COLLECTED ITEMS
   [SerializeField] private Transform collectionPoint;
   //VARIABLES FOR ITEM SPAWNING   
   [SyncVar] int nextItem;
   [SyncVar] int nextLocation;
   //LOCATION TO SPAWN ALERT SYSTEM PREFAB
   [SyncVar] public Transform alertLocation;
   [SerializeField] private GameObject alertPrefab;
   private int nextIndex = 0;
    
   private NetworkManagerHnS room;
   private NetworkManagerHnS Room
   {
      get
      {
         if (room != null) { return room; }
         return room = NetworkManager.singleton as NetworkManagerHnS;
      }
   }

   public static void AddSpawnPoint(Transform transform)
   {
      availableSpawns.Add(transform);
   }
   public static void RemoveSpawnPoint(Transform transform) => availableSpawns.Remove(transform);

   public override void OnStartServer()
   {
      NetworkManagerHnS.OnItemReady += SpawnItems;
   }

   [ServerCallback]
   private void OnDestroy() => NetworkManagerHnS.OnItemReady -= SpawnItems;

   [Server]
   public void SpawnItems()
   {
      for (float i = .5f; i < items.Count;)
      {
         nextItem = Random.Range(0, items.Count);
         nextLocation = Random.Range(0, availableSpawns.Count);
         Transform spawnPoint = availableSpawns.ElementAtOrDefault(nextLocation);

         if (spawnPoint == null)
         {
            Debug.Log($"Missing spawn point for items at {nextLocation}");
            return;
         }

         GameObject itemInstance = Instantiate(items[nextItem].ItemPrefab, availableSpawns[nextLocation].position,
            items[nextItem].ItemPrefab.transform.rotation);
         NetworkServer.Spawn(itemInstance);
         requiredItems.Add(items[nextItem].ItemPrefab);
         items.RemoveAt(nextItem);
         availableSpawns.RemoveAt(nextLocation);
      }
   }
   
   [ClientRpc]
   public void RpcItemUpdate(GameObject item)
   {
      Debug.Log(item.name);
      collectedItems.Add(item.gameObject);
      ItemRemove(item.gameObject);
      //check if the list of collected items matches the required items
      //if true, enable escape
      if (collectedItems.Count == requiredItems.Count)
      {
         CmdAllowEscape();
      }
      
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
