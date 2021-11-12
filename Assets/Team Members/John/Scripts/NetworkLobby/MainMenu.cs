using System;
using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Chat;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace John
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerHnS networkManager = null;

        [Header("UI")] [SerializeField] public GameObject landingPagePanel = null;

        [Header("Settings")] public AudioMixer audioMixer;

        public TMP_Text masterValueText;
        public TMP_Text musicValueText;
        public TMP_Text effectsValueText;

        public Slider mouseSensX;
        public TMP_Text xValue;
        public Slider mouseSensY;
        public TMP_Text yValue;

        private Resolution[] resolutions;

        public TMP_Dropdown resolutionDropdown;

        public void Awake()
        {
            networkManager = FindObjectOfType<NetworkManagerHnS>();
            mouseSensX.value = PlayerPrefs.GetFloat("MouseSensX");
            mouseSensY.value = PlayerPrefs.GetFloat("MouseSensY");
        }

        void Start()
        {
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate +
                                "Hz";
                options.Add(option);

                if (resolutions[i].width == Screen.width &&
                    resolutions[i].height == Screen.height) ;
                {
                    currentResolutionIndex = resolutions.Length;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

       
        public void HostLobby()
        {
            networkManager.StopHost();
            networkManager.StartHost();

            landingPagePanel.SetActive(false);
        }

        #region Audio

        public void SetMasterVolume(float volume)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);


            masterValueText.text = (volume).ToString("P0");
        }

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);

            musicValueText.text = (volume).ToString("P0");
        }

        public void SetEffectsVolume(float volume)
        {
            audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);

            effectsValueText.text = (volume).ToString("P0");
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

        #region Visual

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        #endregion

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}