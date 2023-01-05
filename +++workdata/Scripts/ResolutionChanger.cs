using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionChanger : MonoBehaviour
{
    public TextMeshProUGUI fpsDisplay;

    private int frameCount;
    private float elapsedTime;

    // The dropdown menu to choose the framerate
    public TMP_Dropdown framerateDropdown;

    // An array of possible framerates to lock to
    public int[] possibleFramerates = { 60, 120, 144};

    // Create a slider
    public Slider volumeSlider;


    // Create a dropdown menu
    public TMP_Dropdown resolutionDropdown;

    // Array to store the available resolutions
    private Resolution[] resolutions;

    void Start()
    {
        // Get the current refresh rate of the monitor
        int refreshRate = Screen.currentResolution.refreshRate;

        // Set the framerate to match the refresh rate
        Application.targetFrameRate = refreshRate;



        // Populate the dropdown menu with the possible framerates
        foreach (int framerate in possibleFramerates)
        {
            framerateDropdown.options.Add(new TMP_Dropdown.OptionData(framerate.ToString()));
        }

        // Set the default value of the dropdown menu to the monitor's current refresh rate
        int monitorRefreshRate = Screen.currentResolution.refreshRate;
        framerateDropdown.value = System.Array.IndexOf(possibleFramerates, monitorRefreshRate);
        framerateDropdown.RefreshShownValue();

        // Lock the framerate to the selected value in the dropdown menu
        framerateDropdown.onValueChanged.AddListener(LockFramerate);


        // Set the initial value of the slider to the current volume
        volumeSlider.value = AudioListener.volume;


        // Get the available resolutions
        resolutions = Screen.resolutions;

        // Populate the dropdown with the available resolutions
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // Check if this is the current resolution
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // This function is called when the dropdown value is changed
    public void SetResolution(int resolutionIndex)
    {
        // Set the new resolution
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    // This function is called when the slider value is changed
    public void SetVolume(float volume)
    {
        // Set the new volume
        AudioListener.volume = volume;
    }


    // A function that is called when the dropdown value changes
    void LockFramerate(int selectedFramerate)
    {
        Application.targetFrameRate = possibleFramerates[selectedFramerate];
    }

    public void Vsync()
    {
        // Get the maximum refresh rate of the monitor
        int refreshRate = Screen.currentResolution.refreshRate;

        // Set the refresh rate
        Screen.SetResolution(Screen.width, Screen.height, false, refreshRate);
    }

    public void FullscreenToggle()
    {
        bool isFullscreen = Screen.fullScreen;
        Screen.fullScreen = !isFullscreen;
    }

    private void Update()
    {
        // Count the number of frames and the elapsed time
        frameCount++;
        elapsedTime += Time.deltaTime;

        // Calculate the framerate every second and round it to the nearest whole number
        if (elapsedTime >= .5)
        {
            int framerate = Mathf.RoundToInt(frameCount / elapsedTime);

            // Update the text with the current framerate
            fpsDisplay.text = "FPS: " + framerate;

            // Reset the counters
            frameCount = 0;
            elapsedTime = 0;
        }
    }
}

