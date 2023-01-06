using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class Manager : MonoBehaviour
{
    [Header("Main Menu/InGame")]

    public GameObject settingsMenu;
    public GameObject quitMenu;
    public GameObject mainMenuQ;
    public Toggle fullscreen;
    public Toggle fps;
    public AudioSource uiSound;

    public AudioClip buttonSound;

    public EventSystem eventSystem;
    private int file;

    private string activeSceneName;


    [Header("MainMenu")]
    public GameObject creditsMenu;

    [Header("InGame")]
    public GameObject saveFile1;
    public GameObject saveFile2;
    public GameObject saveFile3;
    public GameObject saveMenu;
    public GameObject pauseMenu;

    public Canvas optionsCanvas;

    private GameObject saveFiles;
    private GameSettings gameSettings;
    private EggHealthRadiation eggHealthRadiation;
    private GameObject player;
    private TextMeshProUGUI savedTimeDate;
    private TMP_InputField fileName;

    public bool sceneSwitch;

    private Vector2 playerPos;

    private Button button;


    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "InGame")
        {
            player = GameObject.Find("Player");
            eggHealthRadiation = GameObject.Find("Bars").GetComponent<EggHealthRadiation>();
        }
        if(SceneManager.GetActiveScene().name == "InGame" || SceneManager.GetActiveScene().name == "MainMenu")
        {
            gameSettings = GameObject.Find("Settings").GetComponent<GameSettings>();
        }
    }

    public void QuitGame(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("QuitGame", button));
        }
        else if (sceneSwitch)
        {
            //Closes the application
            Application.Quit();
        }

        ButtonAnimation(button);
    }

    private void Start()
    {
        sceneSwitch = false;
        activeSceneName = SceneManager.GetActiveScene().name;

        if (SceneManager.GetActiveScene().name == "InGame")
        {
            optionsCanvas.enabled = false;

            //Loads the values from the current loaded file
            LOADFILE(PlayerPrefs.GetInt("CurrentFile"));
        }
        Time.timeScale = 1;

        //Loads the respective setting if it exists, if not it is reset and set to the default setting
        #region SETTINGS
        if (activeSceneName == "MainMenu" || activeSceneName == "InGame")
        {
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
        }
        #endregion SETTINGS

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

        if (SceneManager.GetActiveScene().name == "LoadGame")
        {
            //If there is SaveData, it will be displayed in the respective buttons
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
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            PlayerPrefs.DeleteAll();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (optionsCanvas.enabled)
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

    IEnumerator DelaySwitchScene(string function, Button button)
    {
        ButtonSound();

        yield return new WaitForSeconds(.3f);

        sceneSwitch = true;

        SendMessage(function, button);
    }

    private void ButtonSound()
    {
        uiSound.PlayOneShot(buttonSound);
        uiSound.pitch = Random.Range(.8f, 1.2f);
    }

    private void ButtonAnimation(Button button)
    {
        button.gameObject.transform.DOScale(0.8f, 0.15f).OnComplete(() =>
        {
            button.gameObject.transform.DOScale(1f, 0.15f);
        });
    }



    public void LoadSaveFile(Button button)
    {
        if(button.gameObject == saveFile1)
        {
            file = 1;
        }
        if (button.gameObject == saveFile2)
        {
            file = 2;
        }
        if (button.gameObject == saveFile3)
        {
            file = 3;
        }


        if (!sceneSwitch)
        {
            PlayerPrefs.SetInt("CurrentFile", file);
            StartCoroutine(DelaySwitchScene("QuitGame", button));
        }
        else if (sceneSwitch)
        {
            //The SaveFile selected by pressing one of the buttons is set as the active SaveFile, so that the selected SaveFile is loaded in the GameScene
            SceneManager.LoadScene("InGame");
        }
        ButtonAnimation(button);
    }

    public void BackButton()
    {
        //Returns to the Main Menu Screen
        SceneManager.LoadScene("MainMenu");
    }



    public void SaveMenu(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("SaveMenu", button));
        }
        else if (sceneSwitch)
        {
            if (SceneManager.GetActiveScene().name == "InGame")
            {
                pauseMenu.SetActive(false);
            }


            saveMenu.SetActive(true);
        }

        ButtonAnimation(button);
    }

    public void LoadGame(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("LoadGame", button));
        }
        else if (sceneSwitch)
        {
            SceneManager.LoadScene("LoadGame");
        }

        ButtonAnimation(button);
    }

    public void BackToMainMenu(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("BackToMainMenu", button));
        }
        else if (sceneSwitch)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                settingsMenu.SetActive(false);
                creditsMenu.SetActive(false);
                mainMenuQ.SetActive(false);
            }
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    public void PauseMenu()
    {
        pauseMenu.SetActive(true);
        

        saveMenu.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        mainMenuQ.SetActive(false);

        if (SceneManager.GetActiveScene().name == "InGame")
        {
            quitMenu.SetActive(false);
        }
        

    }

    public void SettingsMenu(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("SettingsMenu", button));
        }
        else if(sceneSwitch)
        {
            if (SceneManager.GetActiveScene().name == "InGame")
            {
                pauseMenu.SetActive(false);
            }

            settingsMenu.SetActive(true);
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    public void QuitMenu(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("QuitMenu", button));
        }
        else if (sceneSwitch)
        {
            if (SceneManager.GetActiveScene().name == "InGame")
            {
                pauseMenu.SetActive(false);
            }

            quitMenu.SetActive(true);
        }

        ButtonAnimation(button);
    }

    public void CreditsMenu(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("CreditsMenu", button));
        }
        else if (sceneSwitch)
        {
            if (SceneManager.GetActiveScene().name == "InGame")
            {
                pauseMenu.SetActive(false);
            }

            creditsMenu.SetActive(true);
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    public void MainMenuQMenu()
    {
        if (SceneManager.GetActiveScene().name == "InGame")
        {
            pauseMenu.SetActive(false);
        }

        mainMenuQ.SetActive(true);
        
    }

    public void ResumeButton(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("ResumeButton", button));
        }
        else if (sceneSwitch)
        {
            //Returns to the GameScene and unpauses the game
            optionsCanvas.enabled = false;
            Time.timeScale = 1;
        }

        ButtonAnimation(button);
    }

    public void MainMenuButton(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("MainMenuButton", button));
        }
        else if (sceneSwitch)
        {
            //Returns to the Main Menu
            SceneManager.LoadScene("MainMenu");
        }

        ButtonAnimation(button);
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