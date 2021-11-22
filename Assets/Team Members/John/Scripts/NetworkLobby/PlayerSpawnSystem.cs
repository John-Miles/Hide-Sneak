using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using John;
using Mirror;
using Mirror.Examples.MultipleMatch;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject guardPlayerPrefab = null;
    [SerializeField] private GameObject thiefPlayerPrefab = null;
    private GameObject playerPrefab;
    public bool player;

    [SerializeField] private static List<Transform> spawnPoints = new List<Transform>();
    
    public static event Action PlayerSpawned;

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
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }
    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    public override void OnStartServer()
    {
        NetworkManagerHnS.OnServerReadied += SpawnPlayer;
        
    }
    [ServerCallback]
    private void OnDestroy() => NetworkManagerHnS.OnServerReadied -= SpawnPlayer;

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
       player = conn.identity.GetComponent<NetworkGamePlayerHnS>().isThief;
        if (player)
            {
                playerPrefab = thiefPlayerPrefab;
            }
            else
            {
                playerPrefab = guardPlayerPrefab;
            }

        nextIndex = Random.Range(1, spawnPoints.Count);

        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

            if (spawnPoint == null)
            {
                Debug.LogError($"Missing spawn point for player {nextIndex}");
                return;
            }

            GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextIndex].position,
                spawnPoints[nextIndex].rotation);
            NetworkServer.AddPlayerForConnection(conn, playerInstance);
            NetworkServer.ReplacePlayerForConnection(conn, playerInstance.gameObject,true);
            spawnPoints.Remove(spawnPoint);
            //nextIndex++;



    }
}
