using System.Collections.Generic;
using UnityEngine;
public class ItemManager : MonoBehaviour
{
   public List<Transform> availableSpawns = new List<Transform>();
   public List<John.ItemBase> items;
   public List<John.ItemBase> collectedItems;
   public List<John.ItemBase> itemsToCollect;
   private int nextItem;
   private int nextLocation;

   public void Awake()
   {
      var spawns = FindObjectsOfType(typeof(ItemSpawner));

      foreach (ItemSpawner spawner in spawns)
      {
         availableSpawns.Add(spawner.transform);
      }

      SpawnItems();
   }

   public void SpawnItems()
   {
      for (int i = availableSpawns.Count - 1; i >= 0; i--)
      {
         nextItem = Random.Range(0, items.Count);
         nextLocation = Random.Range(0, availableSpawns.Count);

         Instantiate(items[nextItem].ItemPrefab, availableSpawns[nextLocation].position,availableSpawns[nextLocation].rotation);
         itemsToCollect.Add(items[nextItem]);
         
         items.RemoveAt(nextItem);
         availableSpawns.RemoveAt(nextLocation);
      }
   }
   
   //add function to add items to collected list of items
   
   //add function to call escape points to activate once collected items is full
}
