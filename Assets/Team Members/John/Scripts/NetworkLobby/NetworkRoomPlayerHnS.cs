using System;
using Mirror;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace John
{
    public class NetworkRoomPlayerHnS : NetworkBehaviour
    {
        [Header("Game Options")] [SerializeField]
        private PlayerBase[] characters = default;

        [SerializeField] private Image[] playerRoleImage = new Image[0];
        private int currentCharacterIndex = 0;

        public Image myRoleImage;
        public Sprite thiefIcon;
        public Sprite guardIcon;
        public Sprite emptyIcon;
        public TMP_Text roleTitle;

        [Header("UI")] [SerializeField] private GameObject lobbyUI = null;
        [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[0];
        [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[0];
        [SerializeField] private Button startGameButton = null;
        [SerializeField] private GameObject loadingScreen;
        
        [Header("Item Controls")]
        [SerializeField] private Text itemText;
        [SerializeField] private Button increaseItem;
        [SerializeField] private Button decreaseItem;
        [SerializeField] private Text requireText;
        [SerializeField] private Button increaseRequired;
        [SerializeField] private Button decreaseRequired;
        

        [SyncVar(hook = nameof(HandleDisplayNameChanged))]
        public string DisplayName = "Loading...";

        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool IsReady = false;
        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool IsThief = false;

        [SyncVar] public int itemCount = 3;
        [SyncVar] public int requiredCount = 1;

        public bool isLeader;

        
            

        public bool IsLeader
        {
            set
            {
                isLeader = value;
                startGameButton.gameObject.SetActive(value);
                
            }

        }

        private NetworkManagerHnS room;

        private NetworkManagerHnS Room
        {
            get
            {
                if (room != null)
                {
                    return room;
                }

                return room = NetworkManager.singleton as NetworkManagerHnS;
            }
        }

        public override void OnStartAuthority()
        {
            CmdSetDisplayName(PlayerNameInput.DisplayName);

            lobbyUI.SetActive(true);
            Room.guard.Clear();
            Room.thief.Clear();
            if (isLeader)
            {
                increaseItem.interactable = true;
                decreaseItem.interactable = true;
                increaseRequired.interactable = true;
                decreaseRequired.interactable = true;
            }
        }

        public override void OnStartClient()
        {
            Room.RoomPlayers.Add(this);
            UpdateDisplay();
        }

        public override void OnStopClient()
        {
            Room.RoomPlayers.Remove(this);
            UpdateDisplay();
        }

        [Command(requiresAuthority = false)]
        public void CmdIncreaseItem()
        {
            RpcIncreaseItem();
        }

        [Command(requiresAuthority = false)]
        public void CmdDecreaseItem()
        {
            RpcDecreaseItem();
        }
        
        
        [Command(requiresAuthority = false)]
        public void CmdIncreaseRequired()
        {
            RpcIncreaseRequired();
        }

        [Command(requiresAuthority = false)]
        public void CmdDecreaseRequired()
        {
            RpcDecreaseRequired();
        }
        
        [ClientRpc]
        public void RpcDecreaseItem()
        {
            foreach (var player in Room.RoomPlayers)
            {
                Room.guard.Clear();
                Room.thief.Clear();
                if (itemCount > 1)
                {
                    if (itemCount <= requiredCount)
                    {
                        
                        Debug.Log("Item counts are the same");
                        CmdDecreaseRequired();
                    }
                    player.itemCount--;
                    
                }
            }
            UpdateDisplay();
        }

        [ClientRpc]
        public void RpcIncreaseItem()
        {
            foreach (var player in Room.RoomPlayers)
            {
                Room.guard.Clear();
                Room.thief.Clear();
                if (itemCount < 10)
                {
                    player.itemCount++;
                }
            }
            UpdateDisplay();
        }
        [ClientRpc]
        public void RpcDecreaseRequired()
        {
            foreach (var player in Room.RoomPlayers)
            {
                Room.guard.Clear();
                Room.thief.Clear();
                if (requiredCount != 1)
                {
                    player.requiredCount--;
                    //if (requiredCount <= itemCount)
                    //{
                        //player.requiredCount--;
                    //}
                }
                //return;

            }
            UpdateDisplay();
        }

        [ClientRpc]
        public void RpcIncreaseRequired()
        {
            foreach (var player in Room.RoomPlayers)
            {
                
                if (requiredCount < 10)
                {
                    Room.guard.Clear();
                    Room.thief.Clear();
                    player.requiredCount++;
                    if (requiredCount > itemCount)
                    {
                        CmdIncreaseItem();
                    }
                }
            }
            UpdateDisplay();
        }

        public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();

        public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

        private void UpdateDisplay()
        {
            if (IsThief)
            {
                myRoleImage.sprite = thiefIcon;
                roleTitle.text = "Thief";
            }
            else
            {
                myRoleImage.sprite = guardIcon;
                roleTitle.text = "Guard";
            }
            
            if (!hasAuthority)
            {
                foreach (var player in Room.RoomPlayers)
                {
                    if (player.hasAuthority)
                    {
                        player.UpdateDisplay();
                        break;
                    }
                }

                return;
            }

            for (int i = 0; i < playerNameTexts.Length; i++)
            {
                playerNameTexts[i].text = "Waiting For Player...";
                playerRoleImage[i].sprite = emptyIcon;
                playerReadyTexts[i].text = string.Empty;

            }

            for (int i = 0; i < Room.RoomPlayers.Count; i++)
            {
                playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
                playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady
                    ? "<color=green>Ready</color>"
                    : "<color=red>Not Ready</color>";
                playerRoleImage[i].sprite = Room.RoomPlayers[i].IsThief
                    ? thiefIcon
                    : guardIcon;
                
                Room.RoomPlayers[i].itemCount = Room.RoomPlayers[0].itemCount;
                Room.RoomPlayers[i].itemText.text = Room.RoomPlayers[0].itemCount.ToString();
                Room.RoomPlayers[i].requiredCount = Room.RoomPlayers[0].requiredCount;
                Room.RoomPlayers[i].requireText.text = Room.RoomPlayers[0].requiredCount.ToString();


            }
            Room.NotifyPlayersOfReadyState();
        }

        public void HandleReadyToStart(bool readyToStart)
        {
            if (!isLeader)
            {
                return;
            }

            startGameButton.interactable = readyToStart;
               
            
        }

        [Command]
        private void CmdSetDisplayName(string displayName)
        {
            DisplayName = displayName;
            
        }

        [Command(requiresAuthority = false)]
        public void CmdReadyUp()
        {
            IsReady = !IsReady;
            RpcClearList();
            //foreach (var player in Room.RoomPlayers)
            //{
            //    UpdateDisplay();
            //}
        }

        [ClientRpc]
        public void RpcClearList()
        {
            foreach (var player in Room.RoomPlayers)
            {
                Room.guard.Clear();
                Room.thief.Clear();
            }
            UpdateDisplay();
        }

        [Command(requiresAuthority = false)]
        public void CmdStartGame()
        {
            if (Room.RoomPlayers[0].connectionToClient != connectionToClient)
            {
                return;
            }
            
            StartCoroutine(Room.StartGame());
            
        }

        [ClientRpc]
        public void RpcLoad()
        {
            loadingScreen.SetActive(true);
        }

        [Command(requiresAuthority = false)]
        public void CmdChangeRole()
        {
            RPCDisplay();
            
        }

        [ClientRpc]
        public void RPCDisplay()
        {
            IsThief = !IsThief;
            foreach (var player in Room.RoomPlayers)
            {
                Room.guard.Clear();
                Room.thief.Clear();
                UpdateDisplay();
            }
        }

        public void ReturnToMenu()
        {
            if (isServer)
            {
                Debug.Log("Shutting down Server");
                room.StopHost();
            }
            if(isLocalPlayer)
            {
                Debug.Log("Disconnecting Client");
                room.StopClient();
            }
            Destroy(gameObject);
            FindObjectOfType<MainMenu>().landingPagePanel.SetActive(true);
        }
    }
}