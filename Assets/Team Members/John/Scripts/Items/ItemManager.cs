using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using John;
using Random = UnityEngine.Random;


public class ItemManager : NetworkBehaviour
{
   public static List<Transform> availableSpawns = new List<Transform>();
   public List<John.ItemBase> items;
   public List<John.ItemBase> collectedItems;
   public List<John.ItemBase> requiredItems;
   [SyncVar]
   public int nextItem;
   [SyncVar]
   public int nextLocation;

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
      for (int i = 0; i < items.Count;)
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
            availableSpawns[nextLocation].rotation);
         NetworkServer.Spawn(itemInstance);
         requiredItems.Add(items[nextItem]);
         items.RemoveAt(nextItem);
         availableSpawns.RemoveAt(nextLocation);
         
         
      }
      
      
      
   }
}
