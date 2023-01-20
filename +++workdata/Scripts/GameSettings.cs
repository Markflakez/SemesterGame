using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Windows;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    //General Sceneaudio
    public AudioMixer mixer;

    //Toggle for fullscreen
    public Toggle fullscreen;
    //Toggle for a FPS diplay
    public Toggle fps;

    //AudioGroup only for music
    public AudioMixerGroup musicc;

    const string MIXER_MUSIC = "musicVolume";
    
    const string MIXER_SFX = "sfxVolume";

    //Check if is Fullscreen
    public bool isFullscreen;

    //FPS display text
    public TextMeshProUGUI fpsDisplay;

    private int frameCount;
    private float elapsedTime;

    //Dropdown-menu to choose the framerate
    public TMP_Dropdown refreshRateDropdown;
    //Dropdown that contains resolutions
    public TMP_Dropdown resolutionDropdown;

    //Slider for SFXVolume
    public Slider sfxVolumeSlider;
    //Slider for MusicVolume
    public Slider musicVolumeSlider;


    private void Awake()
    {
    }
    void Start()
    {

        SetSFXVolume(sfxVolumeSlider.value);
        SetMusicVolume(musicVolumeSlider.value);


        float refreshRate = Screen.currentResolution.refreshRateRatio.numerator;

        //Set the options for the dropdown menu
        List<string> hz = new List<string>();
        if (refreshRate >= 144) hz.Add("144fps");
        if (refreshRate >= 120) hz.Add("120fps");
        if (refreshRate >= 60) hz.Add("60fps");
        if (refreshRate >= 30) hz.Add("30fps");

        refreshRateDropdown.AddOptions(hz);


        int width = Display.main.systemWidth;
        int height = Display.main.systemHeight;

        //Set the options for the dropdown menu
        List<string> res = new List<string>();
        if (width >= 3840 && height >= 2160) res.Add("2160p");
        if (width >= 2560 && height >= 1440) res.Add("1440p");
        if (width >= 1920 && height >= 1080) res.Add("1080p");
        if (width >= 1280 && height >= 720) res.Add("720p");


        resolutionDropdown.AddOptions(res);
        //SetResolution(options.Count -1);

    }
    private void Update()
    {
        //Counts the number of frames and the elapsed time
        frameCount++;
        elapsedTime += Time.deltaTime;

        //Calculates the framerate every second and round it to the nearest whole number
        if (elapsedTime >= .5)
        {
            int framerate = Mathf.RoundToInt(frameCount / elapsedTime);

            //Updates the text with the current framerate
            if (SceneManager.GetActiveScene().name == "InGame")
            {
                fpsDisplay.text = "FPS: " + framerate;
            }

            //Resets the counters
            frameCount = 0;
            elapsedTime = 0;
        }
    }
    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(volume) * 20);
    }
    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(volume) * 20);
    }
    public void FullscreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
        isFullscreen = !isFullscreen;
    }
    public void FPSToggle()
    {
        fpsDisplay.enabled = !fpsDisplay.enabled;
    }
    public void SetRefreshRate(int RefreshRateIndexIndex)
    {
        //Gets the selected resolution from the dropdown menu
        string refreshRate = refreshRateDropdown.options[RefreshRateIndexIndex].text;


        //Sets the resolution based on the selected option
        switch (refreshRate)
        {
            case "144fps":
                Application.targetFrameRate = 144;
                break;
            case "120fps":
                Application.targetFrameRate = 120;
                break;
            case "60fps":
                Application.targetFrameRate = 60;
                break;
            case "24fps":
                Application.targetFrameRate = 30;
                break;
        }
    }
    public void SetResolution(int resolutionIndex)
    {
        //Gets the  resolution from the dropdown menu
        string resolution = resolutionDropdown.options[resolutionIndex].text;

        //Sets the resolution based on the selected option

        switch (resolution)
        {

            case "2160p":
                Screen.SetResolution(3840, 2160, isFullscreen);
                break;
            case "1440p":
                Screen.SetResolution(2560, 1440, isFullscreen);
                break;
            case "1080p":
                Screen.SetResolution(1920, 1080, isFullscreen);
                break;
            case "720p":
                Screen.SetResolution(1280, 720, isFullscreen);
                break;
        }
    }
}


