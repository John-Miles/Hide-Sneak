using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace John
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerHnS networkManager = null;

        [Header("UI")] 
        [SerializeField] private GameObject landingPagePanel = null;

        public void HostLobby()
        {
            networkManager.StartHost();
        
            landingPagePanel.SetActive(false);
        
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }   
}

