using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public AudioClip buttonHover;
    public AudioClip buttonClick;

    private int file;

    private string activeSceneName;
    public GameObject saveFile1;
    public GameObject saveFile2;
    public GameObject saveFile3;

    [Header("MainMenu")]
    public GameObject creditsMenu;
    public GameObject creditsButton;

    [Header("InGame")]
    public GameObject saveMenu;
    public GameObject pauseMenu;
    public GameObject questLog;
    public GameObject questText;
    public GameObject questAvatar;
    public GameObject questHeaderText;
    public GameObject inventoryManager;

    public Item egg;
    public Item sword;

    public Canvas optionsCanvas;

    [Header("InGame/Inventory")]
    public GameObject backDrop;
    public GameObject inventoryMain;
    public GameObject mainMenuBackBoard;

    private float buttonDelay = .15f;
    private GameObject saveFiles;
    private GameSettings gameSettings;
    private EggHealthRadiation eggHealthRadiation;
    private GameObject player;
    private TextMeshProUGUI savedTimeDate;
    private TMP_InputField fileName;

    public bool sceneSwitch;
    private bool delaySwitchScene = false;

    private Vector2 playerPos;

    public string sceneName;


    private void Awake()
    {
        Time.timeScale = 1;
        sceneName = SceneManager.GetActiveScene().name;

        gameSettings = GameObject.Find("Settings").GetComponent<GameSettings>();

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

        LoadFileNames();


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
        DOTween.Init();
        DOTween.defaultTimeScaleIndependent = true;
        DOTween.timeScale = 1;
        sceneSwitch = false;
        

        if (sceneName == "InGame")
        {
            player = GameObject.Find("Player");
            eggHealthRadiation = GameObject.Find("Bars").GetComponent<EggHealthRadiation>();

            optionsCanvas.enabled = true;

            pauseMenu.SetActive(true);
            settingsMenu.SetActive(true);

            //Loads the values from the current loaded file
            LOADFILE(PlayerPrefs.GetInt("CurrentFile"));
        }

    }

    private void LoadFileNames()
    {
        saveFile1.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("FILETIME-" + "1");
        
        saveFile2.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("FILETIME-" + "2");

        saveFile3.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("FILETIME-" + "3");

        PlayerPrefs.Save();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            PlayerPrefs.DeleteAll();
        }

        if(Input.GetKeyUp(KeyCode.L))
        {
            LOAD_INVENTORY();
        }
    }

    private void PauseGame()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    IEnumerator DelaySwitchScene(string function, Button button)
    {
        if (delaySwitchScene == false)
        {
            delaySwitchScene = true;
            ButtonSound();
            

            yield return new WaitForSecondsRealtime(buttonDelay);
            
            exitHover(button);
            sceneSwitch = true;
            SendMessage(function, button);
            delaySwitchScene = false;
        }
    }

    public void DeleteSaveFile(int file)
    {
        PlayerPrefs.DeleteKey("PLAYER_HEALTH-" + file);
        PlayerPrefs.DeleteKey("PLAYER_DAMAGEDEALT-" + file);
        PlayerPrefs.DeleteKey("PLAYER_HEALTH_REMOVED_SEGMENTS-" + file);

        PlayerPrefs.DeleteKey("PLAYER_LOCATION_X-" + file);
        PlayerPrefs.DeleteKey("PLAYER_LOCATION_Y-" + file);

        PlayerPrefs.SetString("FILETIME-" + file, "-Empty-");
        PlayerPrefs.Save();

        LoadFileNames();
    }

    void SAVE_PLAYER_LOCATION(int file)
    {
        //Saves the current Player Position
        PlayerPrefs.SetFloat("PLAYER_LOCATION_X-" + file, player.transform.position.x);
        PlayerPrefs.SetFloat("PLAYER_LOCATION_Y-" + file, player.transform.position.y);
    }

    public void SAVE_INVENTORY(int file)
    {

        foreach (var inventorySlot in inventoryManager.GetComponent<InventoryManager>().inventorySlots)
        {
            if (inventorySlot.gameObject.transform.childCount > 0)
            {
                PlayerPrefs.SetString("Inventory-SLOT-" + file + inventorySlot, inventorySlot.gameObject.name);
                PlayerPrefs.SetString("ITEM_NAME-" + file + inventorySlot, inventorySlot.gameObject.GetComponentInChildren<InventoryItem>().item.itemName);
                PlayerPrefs.SetInt("ITEM_COUNT-" + file + inventorySlot, inventorySlot.gameObject.GetComponentInChildren<InventoryItem>().count);

                Debug.Log(PlayerPrefs.GetString("ITEM_NAME-" + file + inventorySlot));
                Debug.Log(PlayerPrefs.GetInt("ITEM_COUNT-" + file + inventorySlot));
            }
        }
        PlayerPrefs.Save();

        
    }

    public void LOAD_INVENTORY()
    {
        foreach (var inventorySlot in inventoryManager.GetComponent<InventoryManager>().inventorySlots)
        {
            //Destroy(inventorySlot.gameObject.transform.GetChild(0).gameObject);

            if (PlayerPrefs.HasKey("Inventory-SLOT-" + PlayerPrefs.GetInt("CurrentFile") + inventorySlot))
            {
                Debug.Log("ye");
                inventoryManager.GetComponent<InventoryManager>().SpawnNewItem(egg, inventorySlot);
                inventorySlot.gameObject.GetComponentInChildren<Text>().text = PlayerPrefs.GetString("ITEM_COUNT-" + PlayerPrefs.GetInt("CurrentFile") + inventorySlot);
            }
        }
    }

    private void ButtonSound()
    {
        uiSound.PlayOneShot(buttonClick);
        uiSound.pitch = Random.Range(.8f, 1.2f);
    }

    private void ButtonAnimation(Button button)
    {
        exitHover(button);

        Vector3 scale = button.gameObject.transform.localScale;
        
        button.gameObject.transform.DOScale(scale * 0.8f, buttonDelay * .5f).OnComplete(() =>
        {
        button.gameObject.transform.DOScale(scale * 1f, buttonDelay * .5f);
        });

        button.gameObject.transform.DOShakeRotation(buttonDelay, 10, 10, 90, true);
    }

    public void LoadSaveFile(Button button)
    {
        if(button.gameObject.name == "SaveFile-1")
        {
            file = 1;
        }
        if (button.gameObject.name == "SaveFile-2")
        {
            file = 2;
        }
        if (button.gameObject.name == "SaveFile-3")
        {
            file = 3;
        }


        DateTime currentDate = DateTime.Today;
        savedTimeDate = GameObject.Find("savedTimeDate-" + file).GetComponent<TextMeshProUGUI>();

        PlayerPrefs.SetString("FILETIME-" + file, currentDate.ToString("dd/MM/yyyy"));
        savedTimeDate.text = PlayerPrefs.GetString("FILETIME-" + file);


        if (!sceneSwitch)
        {
            PlayerPrefs.SetInt("CurrentFile", file);
            StartCoroutine(DelaySwitchScene("LoadSaveFile", button));
        }
        if (sceneSwitch)
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

    public void enterHover(Button button)
    {
        if (delaySwitchScene == false)
        {
            uiSound.PlayOneShot(buttonHover);
            if (button.gameObject.name != "CreditsButton")
            {
                button.gameObject.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
            }
        }
    }

    public void exitHover(Button button)
    {
        if (button.gameObject.name != "CreditsButton")
        {
            button.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public void OpenInventoryInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OpenInventory();
        }
    }

    public void OpenQuestLogInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OpenQuestLog();
        }
    }


    public void EscapeInput(InputAction.CallbackContext context)
    {
        if (sceneName == "InGame")
        {
            if (context.performed)
            {
                if (!inventoryMain.activeSelf && !questLog.activeSelf)
                {
                    if (optionsCanvas.enabled)
                    {

                        optionsCanvas.enabled = false;
                        pauseMenu.SetActive(true);
                        saveMenu.SetActive(false);
                        quitMenu.SetActive(false);
                        mainMenuQ.SetActive(false);
                        settingsMenu.SetActive(false);
                        
                    }
                    else
                    {
                        optionsCanvas.enabled = true;
                        //pauseMenu.SetActive(true);
                        Time.timeScale = 0;
                    }
                }
                else
                {
                    if(inventoryMain.activeSelf)
                    {
                        OpenInventory();
                    }
                    else if (questLog.activeSelf)
                    {
                        OpenQuestLog();
                    }
                }
            }
        }
    }
    public void OpenInventory()
    {
        if (!questLog.activeSelf)
        {
            if (!backDrop.activeSelf)
            {
                backDrop.SetActive(true);
                inventoryMain.SetActive(true);
                mainMenuBackBoard.SetActive(true);
            }
            else
            {
                backDrop.SetActive(false);
                inventoryMain.SetActive(false);
                mainMenuBackBoard.SetActive(false);
            }
            PauseGame();
        }
    }

    public void OpenQuestLog()
    {
        if (!inventoryMain.activeSelf)
        {
            if (!backDrop.activeSelf)
            {
                backDrop.SetActive(true);
                questLog.SetActive(true);
            }
            else
            {
                backDrop.SetActive(false);
                questLog.SetActive(false);
            }
            PauseGame();
        }
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
            sceneSwitch = false;
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
            sceneSwitch = false;
            LoadFileNames();
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
                creditsButton.SetActive(true);
            }
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    public void PauseMenu(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("PauseMenu", button));
        }
        else if (sceneSwitch)
        {
            pauseMenu.SetActive(true);
            saveMenu.SetActive(false);
            settingsMenu.SetActive(false);
            creditsMenu.SetActive(false);
            mainMenuQ.SetActive(false);

            if (sceneName == "InGame")
            {
                quitMenu.SetActive(false);
            }

        }
        ButtonAnimation(button);
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
            sceneSwitch = false;
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
            creditsButton.SetActive(false);
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
            sceneSwitch = false;
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
            sceneSwitch = false;
        }

        ButtonAnimation(button);
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
        PlayerPrefs.SetFloat("PLAYER_DAMAGEDEALT-" + file, eggHealthRadiation.damageDealt);
        PlayerPrefs.SetFloat("PLAYER_HEALTH_REMOVED_SEGMENTS-" + file, (int)eggHealthRadiation.removedSegments);
    }

    void LOAD_PLAYER_HEALTH(int file)
    {
        //Sets the current Player Health, Health Text and Healthbar to the values from the selected Save File
        eggHealthRadiation.health = PlayerPrefs.GetFloat("PLAYER_HEALTH-" + file);
        eggHealthRadiation.damageDealt = PlayerPrefs.GetFloat("PLAYER_DAMAGEDEALT-" + file);
        eggHealthRadiation.healthMat.SetFloat("_RemovedSegments", PlayerPrefs.GetFloat("PLAYER_HEALTH_REMOVED_SEGMENTS-" + file));


        if(!PlayerPrefs.HasKey("PLAYER_HEALTH-" + file))
        {
            eggHealthRadiation.health = 100;
            eggHealthRadiation.damageDealt = 0;
        }
        eggHealthRadiation.UpdateHealth();
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
        PlayerPrefs.Save();
        LOAD_FPS();
    }
    void RESET_FULLSCREEN()
    {
        PlayerPrefs.SetInt("FULLSCREEN", 0);
        PlayerPrefs.Save();
        LOAD_FULLSCREEN();
    }
    void RESET_FRAMERATE()
    {
        PlayerPrefs.SetInt("FRAMERATE", 0);
        PlayerPrefs.Save();
        LOAD_FRAMERATE();
    }
    void RESET_RESOLUTION()
    {
        PlayerPrefs.SetInt("RESOLUTION", 0);
        PlayerPrefs.Save();
        LOAD_RESOLUTION();
    }
    void RESET_MUSIC_VOLUME()
    {
        PlayerPrefs.SetFloat("MUSIC_VOLUME", 1);
        PlayerPrefs.Save();
        LOAD_MUSIC_VOLUME();
    }
    void RESET_SFX_VOLUME()
    {
        PlayerPrefs.SetFloat("SFX_VOLUME", 1);
        PlayerPrefs.Save();
        LOAD_SFX_VOLUME();
    }
    #endregion RESETSETTINGS
}