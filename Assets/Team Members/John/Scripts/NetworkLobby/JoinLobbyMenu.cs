using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace John
{
    
    public class JoinLobbyMenu : NetworkBehaviour
    {
        [SerializeField] private NetworkManagerHnS networkManager = null;

        [Header("UI")] 
        [SerializeField] private GameObject landingPagePanel;
        [SerializeField] private TMP_InputField ipAddressInputField;
        [SerializeField] private Button joinButton = null;

        private void OnEnable()
        {
            NetworkManagerHnS.onClientConnected += HandleClientConnected;
            NetworkManagerHnS.onClientDisconnected += HandleClientDisconnected;
        }

        private void OnDisable()
        {
            NetworkManagerHnS.onClientConnected -= HandleClientConnected;
            NetworkManagerHnS.onClientDisconnected -= HandleClientDisconnected;
        }

        public void JoinLobby()
        {
            string ipAddress = ipAddressInputField.text;

            networkManager.networkAddress = ipAddress;
            networkManager.StartClient();

            joinButton.interactable = false;
        }

        private void HandleClientConnected()
        {
            joinButton.interactable = true;
        
            gameObject.SetActive(false);
            landingPagePanel.SetActive(false);
        }

        private void HandleClientDisconnected()
        {
            joinButton.interactable = true;
        }
    }

}