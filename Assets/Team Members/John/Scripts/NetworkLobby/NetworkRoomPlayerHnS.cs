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
        public TMP_Text roleTitle;

        [Header("UI")] [SerializeField] private GameObject lobbyUI = null;
        [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[0];
        [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[0];
        [SerializeField] private Button startGameButton = null;

        [SerializeField] private GameObject loadingScreen;

        [SyncVar(hook = nameof(HandleDisplayNameChanged))]
        public string DisplayName = "Loading...";

        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool IsReady = false;
        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool IsThief = false;

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
            foreach (var player in Room.RoomPlayers)
            {
                Room.guard.Clear();
                Room.thief.Clear();
                UpdateDisplay();
            }
        }

        [Command(requiresAuthority = false)]
        public void CmdStartGame()
        {
            if (Room.RoomPlayers[0].connectionToClient != connectionToClient)
            {
                return;
            }
            RpcLoad();
            Room.StartGame();
            
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
          NetworkClient.Shutdown();
          FindObjectOfType<MainMenu>().landingPagePanel.SetActive(true);
        }
    }
}