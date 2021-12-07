using System.Collections;
using System.Collections.Generic;
using John;
using Mirror;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : NetworkBehaviour
{
    public GameObject network;
    [Header("Settings")] public AudioMixer audioMixer;

    public TMP_Text masterValueText;
    public TMP_Text musicValueText;
    public TMP_Text effectsValueText;

    public Slider mouseSensX;
    public TMP_Text xValue;
    public Slider mouseSensY;
    public TMP_Text yValue;

    private const string resolutionWidthPlayerPrefKey = "ResolutionWidth";
    private const string resolutionHeightPlayerPrefKey = "ResolutionHeight";
    private const string resolutionRefreshRatePlayerPrefKey = "RefreshRate";
    private const string fullScreenPlayerPrefKey = "FullScreen";
    public Toggle fullScreenToggle;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    Resolution selectedResolution;

    public GameObject pauseMenu;

    public void Awake()
    {
        SetSensitivity();
        SetAudioVolumes();
        network = FindObjectOfType<NetworkManagerHnS>().gameObject;
    }

    public void SetSensitivity()
    {
        mouseSensX.value = PlayerPrefs.GetFloat("MouseSensX", 5);
        mouseSensY.value = PlayerPrefs.GetFloat("MouseSensY", 5);
    }

    void SetAudioVolumes()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("PPMasterVolume", 1));
        Debug.Log(PlayerPrefs.GetFloat("PPMasterVolume") + " on start Master Volume player prefs");
        SetMusicVolume(PlayerPrefs.GetFloat("PPMusicVolume", 1));
        SetEffectsVolume(PlayerPrefs.GetFloat("PPEffectsVolume", 1));
    }

    void Start()
    {
        resolutions = Screen.resolutions;
        LoadSettings();

        CreateResolutionDropdown();

        fullScreenToggle.onValueChanged.AddListener(SetFullScreen);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }


    #region Audio

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

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
        PlayerPrefs.SetFloat("MouseSensX", mouseSensX.value);

        xValue.text = mouseSensX.value.ToString("F");

        SetSensitivity();
    }

    public void SetMouseSensitivityY()
    {
        PlayerPrefs.SetFloat("MouseSensY", mouseSensY.value);

        yValue.text = mouseSensY.value.ToString("F");

        SetSensitivity();
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
        selectedResolution.height = PlayerPrefs.GetInt(resolutionHeightPlayerPrefKey, Screen.currentResolution.height);
        selectedResolution.refreshRate =
            PlayerPrefs.GetInt(resolutionRefreshRatePlayerPrefKey, Screen.currentResolution.refreshRate);

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

    public void ReturnToMenu()
    {
        Destroy(network);
        if (isClient)
        {
            NetworkClient.Disconnect();
        }

        if (isServer)
        {
            NetworkClient.Disconnect();
            NetworkServer.Shutdown();
        }

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}