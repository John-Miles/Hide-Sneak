using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Chat;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace John
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Networking")] [SerializeField]
        private NetworkManagerHnS networkManager = null;

        [SerializeField] private GameObject networkManagerPrefab;

        [Header("UI")] [SerializeField] public GameObject landingPagePanel = null;

        [Header("Settings")] [Header("Audio")] public AudioMixer audioMixer;
        public TMP_Text masterValueText;
        public Slider masterSlider;
        public TMP_Text musicValueText;
        public Slider musicSlider;
        public TMP_Text effectsValueText;
        public Slider effectsSlider;
        [Header("Controls")] public Slider mouseSensX;
        public TMP_Text xValue;
        public Slider mouseSensY;
        public TMP_Text yValue;
        [Header("Graphics")] private const string resolutionWidthPlayerPrefKey = "ResolutionWidth";
        private const string resolutionHeightPlayerPrefKey = "ResolutionHeight";
        private const string resolutionRefreshRatePlayerPrefKey = "RefreshRate";
        private const string fullScreenPlayerPrefKey = "FullScreen";
        public Toggle fullScreenToggle;
        public TMP_Dropdown resolutionDropdown;
        Resolution[] resolutions;
        Resolution selectedResolution;


        public void Awake()
        {
            SetSensitivity();
            SetAudioVolumes();
            if (!networkManager)
            {
                Instantiate(networkManagerPrefab);
            }

            networkManager = FindObjectOfType<NetworkManagerHnS>();
            NetworkServer.Shutdown();
            Cursor.lockState = CursorLockMode.None;
            mouseSensX.value = PlayerPrefs.GetFloat("MouseSensX", 5);
            xValue.text = mouseSensX.value.ToString("F");
            mouseSensY.value = PlayerPrefs.GetFloat("MouseSensY", 5);
            yValue.text = mouseSensY.value.ToString("F");
        }

        public void SetSensitivity()
        {
            mouseSensX.value = PlayerPrefs.GetFloat("MouseSensX", 5);
            mouseSensY.value = PlayerPrefs.GetFloat("MouseSensY", 5);
        }

        void Start()
        {
            resolutions = Screen.resolutions;
            LoadSettings();

            CreateResolutionDropdown();

            fullScreenToggle.onValueChanged.AddListener(SetFullScreen);
            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }

        public void HostLobby()
        {
            networkManager.StopHost();
            networkManager.StartHost();
            landingPagePanel.SetActive(false);
        }

        #region Audio

        void SetAudioVolumes()
        {
            SetMasterVolume(PlayerPrefs.GetFloat("PPMasterVolume", 1));
            masterSlider.value = PlayerPrefs.GetFloat("MasterSliderValue", 1);
            
            SetMusicVolume(PlayerPrefs.GetFloat("PPMusicVolume", 1));
            SetEffectsVolume(PlayerPrefs.GetFloat("PPEffectsVolume", 1));
        }

        public void SetMasterVolume(float volume)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

            float sliderValue = masterSlider.value;
            
            PlayerPrefs.SetFloat("MasterSliderValue", sliderValue);

            PlayerPrefs.SetFloat("PPMasterVolume", volume);


            masterValueText.text = (volume).ToString("P0");
        }

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);

            PlayerPrefs.SetFloat("PPMusicVolume", volume);

            musicValueText.text = (volume).ToString("P0");
        }

        public void SetEffectsVolume(float volume)
        {
            audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);

            PlayerPrefs.SetFloat("PPEffectsVolume", volume);

            effectsValueText.text = (volume).ToString("P0");
        }

        #endregion

        #region Controls

        public void SetMouseSensitivityX()
        {
            xValue.text = mouseSensX.value.ToString("F");

            PlayerPrefs.SetFloat("MouseSensX", mouseSensX.value);
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
            selectedResolution = resolutions[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt(resolutionWidthPlayerPrefKey, selectedResolution.width);
            PlayerPrefs.SetInt(resolutionHeightPlayerPrefKey, selectedResolution.height);
            PlayerPrefs.SetInt(resolutionRefreshRatePlayerPrefKey, selectedResolution.refreshRate);
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
            PlayerPrefs.SetInt(fullScreenPlayerPrefKey, isFullScreen ? 1 : 0);
        }

        private void LoadSettings()
        {
            selectedResolution = new Resolution();
            selectedResolution.width = PlayerPrefs.GetInt(resolutionWidthPlayerPrefKey, Screen.currentResolution.width);
            selectedResolution.height =
                PlayerPrefs.GetInt(resolutionHeightPlayerPrefKey, Screen.currentResolution.height);
            selectedResolution.refreshRate = PlayerPrefs.GetInt(resolutionRefreshRatePlayerPrefKey,
                Screen.currentResolution.refreshRate);

            fullScreenToggle.isOn = PlayerPrefs.GetInt(fullScreenPlayerPrefKey, Screen.fullScreen ? 1 : 0) > 0;

            Screen.SetResolution(
                selectedResolution.width,
                selectedResolution.height,
                fullScreenToggle.isOn
            );
        }

        private void CreateResolutionDropdown()
        {
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate +
                                "Hz";
                options.Add(option);
                if (Mathf.Approximately(resolutions[i].width, selectedResolution.width) &&
                    Mathf.Approximately(resolutions[i].height, selectedResolution.height))
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        #endregion

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}