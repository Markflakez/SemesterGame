using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

public class Manager : MonoBehaviour
{
    private GameObject player;
    private GameObject saveFiles;
    private GameSettings gameSettings;
    private EggHealthRadiation eggHealthRadiation;

    public Canvas optionsCanvas;

    public GameObject saveMenu;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject quitMenu;
    public GameObject creditsMenu;
    public GameObject mainMenuQ;

    public GameObject saveFile1;
    public GameObject saveFile2;
    public GameObject saveFile3;

    private TextMeshProUGUI savedTimeDate;
    private TMP_InputField fileName;

    public Toggle fullscreen;
    public Toggle fps;

    private Vector2 playerPos;

    private void Awake()
    {
        player = GameObject.Find("Player");
        eggHealthRadiation = GameObject.Find("Bars").GetComponent<EggHealthRadiation>();
        gameSettings = GameObject.Find("Settings").GetComponent<GameSettings>();
    }

    public void QuitGame()
    {
        //Closes the application
        Application.Quit();
    }

    private void Start()
    {
        optionsCanvas.enabled = false;
        Time.timeScale = 1;

        //Loads the respective setting if it exists, if not it is reset and set to the default setting
        #region SETTINGS
        if (PlayerPrefs.HasKey("RESOLUTION"))
        {
            LOAD_RESOLUTION();
        }
        else
        {
            RESET_RESOLUTION();
        }

        if (PlayerPrefs.HasKey("FRAMERATE"))
        {
            LOAD_FRAMERATE();
        }
        else
        {
            RESET_FRAMERATE();
        }

        if (PlayerPrefs.HasKey("FULLSCREEN"))
        {
            LOAD_FULLSCREEN();
        }
        else
        {
            RESET_FULLSCREEN();
        }

        if (PlayerPrefs.HasKey("MUSIC_VOLUME"))
        {
            LOAD_MUSIC_VOLUME();
        }
        else
        {
            RESET_MUSIC_VOLUME();
        }

        if (PlayerPrefs.HasKey("SFX_VOLUME"))
        {
            LOAD_SFX_VOLUME();
        }
        else
        {
            RESET_SFX_VOLUME();
        }

        if (PlayerPrefs.HasKey("FPS"))
        {
            LOAD_FPS();
        }
        else
        {
            RESET_FPS();
        }
        #endregion SETTINGS

        //Loads the values from the current loaded file
        LOADFILE(PlayerPrefs.GetInt("CurrentFile"));

        //Displays the names of the Save Files
        #region LOAD-FILENAMES
        if (PlayerPrefs.HasKey("FILETIME-1"))
        {
            saveFile1.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("FILETIME-" + "1");
        }
        if (PlayerPrefs.HasKey("FILETIME-2"))
        {
            saveFile2.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("FILETIME-" + "2");
        }
        if (PlayerPrefs.HasKey("FILETIME-3"))
        {
            saveFile3.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("FILETIME-" + "3");
        }
        #endregion LOAD-FILENAMES
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            PlayerPrefs.DeleteAll();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(optionsCanvas.enabled)
            {
                optionsCanvas.enabled = false;
                Time.timeScale = 1;
            }
            else
            {
                optionsCanvas.enabled = true;
                Time.timeScale = 0;
            }
        }
    }

    public void SaveMenu()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        quitMenu.SetActive(false);
        creditsMenu.SetActive(false);
        mainMenuQ.SetActive(false);

        saveMenu.SetActive(true);
        
    }
    public void PauseMenu()
    {
        saveMenu.SetActive(false);
        settingsMenu.SetActive(false);
        quitMenu.SetActive(false);
        creditsMenu.SetActive(false);
        mainMenuQ.SetActive(false);

        pauseMenu.SetActive(true);
        
    }

    public void SettingsMenu()
    {
        saveMenu.SetActive(false);
        pauseMenu.SetActive(false);
        quitMenu.SetActive(false);
        creditsMenu.SetActive(false);
        mainMenuQ.SetActive(false);

        settingsMenu.SetActive(true);
    }

    public void QuitMenu()
    {
        saveMenu.SetActive(false);
        pauseMenu.SetActive(false);
        quitMenu.SetActive(false);
        creditsMenu.SetActive(false);
        settingsMenu.SetActive(false);

        quitMenu.SetActive(true);
    }

    public void CreditsMenu()
    {
        saveMenu.SetActive(false);
        pauseMenu.SetActive(false);
        quitMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenuQ.SetActive(false);

        creditsMenu.SetActive(true);
    }

    public void MainMenuQMenu()
    {
        saveMenu.SetActive(false);
        pauseMenu.SetActive(false);
        quitMenu.SetActive(false);
        creditsMenu.SetActive(false);
        settingsMenu.SetActive(false);

        mainMenuQ.SetActive(true);
    }

    public void ResumeButton()
    {
        //Returns to the GameScene and unpauses the game
        optionsCanvas.enabled = false;
        Time.timeScale = 1;
    }

    public void MainMenuButton()
    {
        //Returns to the Main Menu
        SceneManager.LoadScene("MainMenu");
    }

    public void NewGame()
    {
        eggHealthRadiation.health = 100;
        eggHealthRadiation.healthSegments = (int)eggHealthRadiation.healthMat.GetFloat("_SegmentCount");
        eggHealthRadiation.healthMat.SetFloat("_RemovedSegments", eggHealthRadiation.removedSegments);

        eggHealthRadiation.healthText.text = "";
        eggHealthRadiation.healthText.text += eggHealthRadiation.health;
    }


    public void Save()
    {
        //Saves the current Player Health and the current state of the Health Bar
        #region PlayerHealth
        PlayerPrefs.SetFloat("PLAYER_HEALTH", eggHealthRadiation.health);
        PlayerPrefs.SetFloat("PLAYER_HEALTH_REMOVED_SEGMENTS", eggHealthRadiation.removedSegments);
        #endregion PlayerHealth

        //Saves the current location of the player
        #region PlayerLocation
        PlayerPrefs.SetFloat("PLAYER_LOCATION_X", player.transform.position.x);
        PlayerPrefs.SetFloat("PLAYER_LOCATION_Y", player.transform.position.y);
        #endregion PlayerLocation
    }



    public void SAVEFILE(int file)
    {
        DateTime currentDate = DateTime.Today;
        savedTimeDate = GameObject.Find("savedTimeDate-" + file).GetComponent<TextMeshProUGUI>();

        PlayerPrefs.SetString("FILETIME-" + file, currentDate.ToString("dd/MM/yyyy"));
        savedTimeDate.text = PlayerPrefs.GetString("FILETIME-" + file);
        
        //Saves the current Player Health and Position
        SAVE_PLAYER_HEALTH(file);
        SAVE_PLAYER_LOCATION(file);
    }

    public void LOADFILE(int file)
    {
        //savedTimeDate = GameObject.Find("savedTimeDate-" + file).GetComponent<TextMeshProUGUI>();
        //fileName = GameObject.Find("FileName-" + file).GetComponent<TMP_InputField>();

        //savedTimeDate.text = PlayerPrefs.GetString("FILETIME-" + file);
        //fileName.text = PlayerPrefs.GetString("FILENAME-" + file);

        LOAD_PLAYER_HEALTH(file);
        LOAD_PLAYER_LOCATION(file);

    }

    void SAVE_PLAYER_HEALTH(int file)
    {
        //Saves the current Player Health
        PlayerPrefs.SetFloat("PLAYER_HEALTH-" + file, eggHealthRadiation.health);
        PlayerPrefs.SetFloat("PLAYER_HEALTH_REMOVED_SEGMENTS-" + file, eggHealthRadiation.removedSegments);
    }

    void SAVE_PLAYER_LOCATION(int file)
    {
        //Saves the current Player Position
        PlayerPrefs.SetFloat("PLAYER_LOCATION_X-" + file, player.transform.position.x);
        PlayerPrefs.SetFloat("PLAYER_LOCATION_Y-" + file, player.transform.position.y);
    }

    void LOAD_PLAYER_HEALTH(int file)
    {
        //Sets the current Player Health, Health Text and Healthbar to the values from the selected Save File
        eggHealthRadiation.health = PlayerPrefs.GetFloat("PLAYER_HEALTH-" + file);
        eggHealthRadiation.healthText.text = "";
        eggHealthRadiation.healthText.text += eggHealthRadiation.health;
        eggHealthRadiation.removedSegments = PlayerPrefs.GetFloat("PLAYER_HEALTH_REMOVED_SEGMENTS-" + file);
    }

    void LOAD_PLAYER_LOCATION(int file)
    {
        //Sets the current Player Position to the values from the selected Save File
        player.transform.position = new Vector2(PlayerPrefs.GetFloat("PLAYER_LOCATION_X-" + file), PlayerPrefs.GetFloat("PLAYER_LOCATION_Y-" + file));
    }

    public void ResetSettings()
    {
        //Resets all the Settings to their default value
        RESET_FPS();
        RESET_FRAMERATE();
        RESET_FULLSCREEN();
        RESET_MUSIC_VOLUME();
        RESET_SFX_VOLUME();
        RESET_RESOLUTION();
    }

    //Saves the respective setting
    #region SAVESETTINGS
    public void SAVE_FPS()
    {
        if (fps.isOn)
        {
            PlayerPrefs.SetInt("FPS", 1);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("FPS", 0);
            PlayerPrefs.Save();
        }
    }
    public void SAVE_FULLSCREEN()
    {
        if (gameSettings.isFullscreen)
        {
            PlayerPrefs.SetInt("FULLSCREEN", 0);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("FULLSCREEN", 1);
            PlayerPrefs.Save();
        }

    }
    public void SAVE_FRAMERATE(int framerate)
    {
        PlayerPrefs.SetInt("FRAMERATE", framerate);
        PlayerPrefs.Save();
    }
    public void SAVE_RESOLUTION(int resolution)
    {
        PlayerPrefs.SetInt("RESOLUTION", resolution);
        PlayerPrefs.Save();
    }
    public void SAVE_MUSIC_VOLUME(float volume)
    {
        PlayerPrefs.SetFloat("MUSIC_VOLUME", volume);
        PlayerPrefs.Save();
    }
    public void SAVE_SFX_VOLUME(float volume)
    {
        PlayerPrefs.SetFloat("SFX_VOLUME", volume);
        PlayerPrefs.Save();
    }
    #endregion SAVESETTINGS

    //Loads the respective setting
    #region LOADSETTINGS
    void LOAD_FPS()
    {
        if (PlayerPrefs.GetInt("FPS") == 1)
        {
            fps.isOn = true;
            gameSettings.fpsDisplay.enabled = true;
        }
        else
        {
            fps.isOn = false;
            gameSettings.fpsDisplay.enabled = false;
        }
    }
    void LOAD_FULLSCREEN()
    {
        if (PlayerPrefs.GetInt("FULLSCREEN") == 0)
        {
            fullscreen.isOn = true;
            Screen.fullScreen = true;
            gameSettings.isFullscreen = true;
        }
        else
        {
            fullscreen.isOn = false;
            Screen.fullScreen = false;
            gameSettings.isFullscreen = false;
        }
    }
    void LOAD_FRAMERATE()
    {
        gameSettings.refreshRateDropdown.value = PlayerPrefs.GetInt("FRAMERATE");
        gameSettings.SetRefreshRate(PlayerPrefs.GetInt("FRAMERATE"));
    }
    void LOAD_RESOLUTION()
    {
        gameSettings.resolutionDropdown.value = PlayerPrefs.GetInt("RESOLUTION");
        gameSettings.SetResolution(PlayerPrefs.GetInt("RESOLUTION"));
    }
    void LOAD_MUSIC_VOLUME()
    {
        gameSettings.musicVolumeSlider.value = PlayerPrefs.GetFloat("MUSIC_VOLUME");
    }
    void LOAD_SFX_VOLUME()
    {
        gameSettings.sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFX_VOLUME");
    }
    #endregion LOADSETTINGS

    //Resets and loads the respective setting
    #region RESETSETTINGS
    void RESET_FPS()
    {
        PlayerPrefs.SetInt("FPS", 0);
        LOAD_FPS();
    }
    void RESET_FULLSCREEN()
    {
        PlayerPrefs.SetInt("FULLSCREEN", 0);
        LOAD_FULLSCREEN();
    }
    void RESET_FRAMERATE()
    {
        PlayerPrefs.SetInt("FRAMERATE", 0);
        LOAD_FRAMERATE();
    }
    void RESET_RESOLUTION()
    {
        PlayerPrefs.SetInt("RESOLUTION", 0);
        LOAD_RESOLUTION();
    }
    void RESET_MUSIC_VOLUME()
    {
        PlayerPrefs.SetFloat("MUSIC_VOLUME", 1);
        LOAD_MUSIC_VOLUME();
    }
    void RESET_SFX_VOLUME()
    {
        PlayerPrefs.SetFloat("SFX_VOLUME", 1);
        LOAD_SFX_VOLUME();
    }
    #endregion RESETSETTINGS

}