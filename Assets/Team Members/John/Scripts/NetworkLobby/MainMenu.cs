using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Chat;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace John
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerHnS networkManager = null;

        [Header("UI")] 
        [SerializeField] private GameObject landingPagePanel = null;
        
        [Header("Settings")]
        public AudioMixer audioMixer;

        public TMP_Text masterValueText;
        public TMP_Text musicValueText;
        public TMP_Text effectsValueText;

        public Slider mouseSensX;
        public TMP_Text xValue;
        public Slider mouseSensY;
        public TMP_Text yValue;

        public void HostLobby()
        {
            networkManager.StartHost();
        
            landingPagePanel.SetActive(false);
        
        }

        #region AUDIO
        
       public void SetMasterVolume(float volume)
       {
           audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

          // masterValueText.text = audioMixer.GetFloat("MasterVolume");
       }

       public void SetMusicVolume(float volume)
       {
           audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
       }

       public void SetEffectsVolume(float volume)
       {
           audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
       }
       
       #endregion

       #region Controls

       public void SetMouseSensitivityX()
       {
           PlayerPrefs.SetFloat("MouseSensX", mouseSensX.value);
           
           xValue.text = mouseSensX.value.ToString("F");
       }

       public void SetMouseSensitivityY()
       {
           PlayerPrefs.SetFloat("MouseSensY", mouseSensY.value);
           
           yValue.text = mouseSensY.value.ToString("F");
       }
       

       #endregion
        public void QuitGame()
        {
            Application.Quit();
        }
    }   
}

