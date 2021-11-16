using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace John
{

    public class NetworkManagerHnS : NetworkManager
    {
        [SerializeField] private int minPlayers = 2;
        [Scene] [SerializeField] private string menuScene = string.Empty;

        [Header("Room")] 
        [SerializeField] private NetworkRoomPlayerHnS roomPlayerPrefab = null;

        [Header("Game")] 
        [SerializeField] private NetworkGamePlayerHnS gamePlayerPrefab = null;
        [SerializeField] private GameObject playerSpawnSystem = null;
        

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;

        public static event Action OnItemReady;
        
        [SerializeField] public List<NetworkRoomPlayerHnS> thief = new List<NetworkRoomPlayerHnS>();
        [SerializeField] public List<NetworkRoomPlayerHnS> guard = new List<NetworkRoomPlayerHnS>();
        

        [SerializeField]public List<NetworkRoomPlayerHnS> RoomPlayers { get; } = new List<NetworkRoomPlayerHnS>();
        [SerializeField]public List<NetworkGamePlayerHnS> GamePlayers { get; } = new List<NetworkGamePlayerHnS>();
        
        public override void OnStartServer()
        {
            spawnPrefabs.Clear();
            spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
        }
 
        public override void OnStartClient()
        {
            spawnPrefabs.Clear();
            spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
 
            NetworkClient.ClearSpawners();
 
            foreach (var prefab in spawnPrefabs)
            {
                NetworkClient.RegisterPrefab(prefab);
            }
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            OnClientConnected?.Invoke();
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            Debug.Log("Disconnected from Server");
            OnClientDisconnected?.Invoke();
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            if (numPlayers >= maxConnections)
            {
                conn.Disconnect();
                return;
            }

            if (SceneManager.GetActiveScene().path != menuScene)
            {
                conn.Disconnect();
                return;
            }
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            if (SceneManager.GetActiveScene().path == menuScene)
            {
                bool isLeader = RoomPlayers.Count == 0;

                NetworkRoomPlayerHnS roomPlayerInstance = Instantiate(roomPlayerPrefab);

                roomPlayerInstance.isLeader = isLeader;

                NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
            }
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayerHnS>();
                RoomPlayers.Remove(player);

                NotifyPlayersOfReadyState();
            }

            base.OnServerDisconnect(conn);
        }

        public override void OnStopServer()
        {
            RoomPlayers.Clear();
        }

        public void NotifyPlayersOfReadyState()
        {
            for (int i = 0; i < RoomPlayers.Count; i++)
            {
                if (RoomPlayers[i].IsThief)
                {
                    thief.Add(RoomPlayers[i]);
                }
                
                if(!RoomPlayers[i].IsThief)
                {
                    guard.Add(RoomPlayers[i]);
                }
            }

           
            
            foreach (var players in RoomPlayers)
            {
                
                
                    players.HandleReadyToStart(IsReadyToStart());
                
            }
        }

        private bool IsReadyToStart()
        {
            if (numPlayers < minPlayers)
            {
                return false;
            }

            
            
            foreach (var player in RoomPlayers)
            {
                if (!player.IsReady)
                {
                    return false;
                }
            }
            return true;
            //return (guard.Count >= 1 && thief.Count >= 1);
        }

        public IEnumerator StartGame()
        {
            if (SceneManager.GetActiveScene().path == menuScene)
            {
                if(!IsReadyToStart()) {yield return null;}

                foreach (var player in guard)
                {
                    player.RpcLoad();
                }

                foreach (var player in thief)
                {
                    player.RpcLoad();
                }

                yield return new WaitForSeconds(2f);
                
                ServerChangeScene("Gameplay_Level_1");
            }
        }


        public override void ServerChangeScene(string newSceneName)
        {
            //From the menu scene to the game scene
            if (SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Gameplay_Level"))
            {
                for (int i = RoomPlayers.Count - 1; i >= 0; i--)
                {
                    var conn = RoomPlayers[i].connectionToClient;
                    var gameplayerInstance = Instantiate(gamePlayerPrefab);
                    gameplayerInstance.SetCount(RoomPlayers[i].itemCount,RoomPlayers[i].requiredCount);
                    gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);
                    gameplayerInstance.SetRole(RoomPlayers[i].IsThief);
                    
                    
                    Debug.Log("Now spawning " + RoomPlayers[i].DisplayName);
                    
                    NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject,true);
                    //NetworkServer.Destroy(conn.identity.gameObject);
                }
                
                base.ServerChangeScene(newSceneName);
            }
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName.StartsWith("Gameplay_Level"))
            {
                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
                NetworkServer.Spawn(playerSpawnSystemInstance);
            }
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            base.OnServerReady(conn);
            
            OnServerReadied?.Invoke(conn);
            OnItemReady?.Invoke();
            
            
        }
    }
}