using Mirror;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace John
{
    public class NetworkGamePlayerHnS : NetworkBehaviour
    {
        [SyncVar]
        private string displayName = "Loading...";
        private NetworkManagerHnS room;
        private NetworkManagerHnS Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerHnS;
            }
        }

        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);
            Room.GamePlayers.Add(this);
        }

        public override void OnStopClient()
        {
            Room.GamePlayers.Remove(this);
        }
        [Server]
        public void SetDisplayName(string displayName)
        {
            this.displayName = displayName;
        }
       
    }
}

