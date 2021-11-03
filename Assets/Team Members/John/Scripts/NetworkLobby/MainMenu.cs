using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace John
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerHnS networkManager = null;

        [Header("UI")] 
        [SerializeField] private GameObject landingPagePanel = null;
        
        [Header("Settings")]
        public AudioMixer audioMixer;

        public void HostLobby()
        {
            networkManager.StartHost();
        
            landingPagePanel.SetActive(false);
        
        }
       public void SetVolume(float volume)
       {
           audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
       }
        public void QuitGame()
        {
            Application.Quit();
        }
    }   
}

