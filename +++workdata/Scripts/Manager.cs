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
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class Manager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    //All three Files for saving/loading gameStates
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
    public InventoryManager inventoryManager;

    public Item egg;
    public Item sword;

    public Canvas optionsCanvas;

    [Header("InGame/Inventory")]
    public GameObject backDrop;
    public GameObject inventoryMain;
    public GameObject inventoryHotbar;
    public GameObject inventoryHotbarBackBoard;
    public GameObject mainMenuBackBoard;

    public GameObject graphicsPanel;
    public GameObject controlsPanel;
    public GameObject audioPanel;


    private float buttonDelay = .15f;
    private GameObject saveFiles;
    private GameSettings gameSettings;
    public GameObject dialogBox;
    public EggHealthRadiation eggHealthRadiation;
    public GameObject player;
    private TextMeshProUGUI savedTimeDate;
    private TMP_InputField fileName;

    public bool sceneSwitch = false;
    private bool delaySwitchScene = false;

    private Vector2 playerPos;

    public Volume postProcessingVolume;
    private ColorAdjustments colorAdjustments;
    public RectTransform uiPanel;

    public string sceneName;

    public InputActionAsset inputActions;

    public Item[] items;
    private Item currentItem;

    public bool isPaused = false;

    public GameObject eggIndicator;

    public bool canThrowEgg = true;

    public GameObject barIndicators;


    public GameObject graphicsButton;
    public GameObject controlsButton;
    public GameObject audioButton;

    public GameObject healthPopup;
    public GameObject eggsPopup;
    public GameObject radiationPopup;

    [HideInInspector]
    public Transform originalSlot;


    private void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;

        DOTween.Init();
        DOTween.defaultTimeScaleIndependent = true;
        DOTween.timeScale = 1;
        Time.timeScale = 1;

        if (sceneName != "LoadGame")
        {
            gameSettings = GameObject.Find("Settings").GetComponent<GameSettings>();
        }
        if (sceneName != "MainMenu")
        {
            LoadFileNames();
        }
        FindInputActions();
    }

    private void Start()
    {

        if(Application.isEditor)
        {
            PlayerPrefs.SetInt("CurrentFile", 1);
        }

        if (sceneName == "InGame")
        {
            LOADFILE();
            
            if(Application.isEditor)
            {
                SaturationFade();
                eggHealthRadiation.RadiationOverTime();
            }
            else
            {
                BlackInFade();
            }
        }

        if (sceneName != "LoadGame")
        {
            LoadSettings();
            Debug.Log("hallo");
        }
        
    }


    public void SaturationFade()
    {
        if (postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.saturation.value = -100;
            DOTween.To(() => colorAdjustments.saturation.value, x => colorAdjustments.saturation.value = x, 10, 5f);
        }
    }
    public void BlackInFade()
    {
        Vector3 startPos = uiPanel.anchoredPosition;
        startPos.y = -500;
        uiPanel.anchoredPosition = startPos;

        if (postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.value = -6;
            DOTween.To(() => colorAdjustments.postExposure.value, x => colorAdjustments.postExposure.value = x, (float)-0.2, 7.5f).OnComplete(SlideInUi);
        }
    }
    private void SlideInUi()
    {
        Vector3 startPos = uiPanel.anchoredPosition;
        startPos.y = -300;
        uiPanel.anchoredPosition = startPos;
        uiPanel.DOAnchorPosY(0, 3.5f).OnComplete(eggHealthRadiation.RadiationOverTime);
    }

    private void HideAllPanels()
    {
        pauseMenu.SetActive(false);
        saveMenu.SetActive(false);
        quitMenu.SetActive(false);
        mainMenuQ.SetActive(false);
        settingsMenu.SetActive(false);
    }
    private void LoadFileNames()
    {
        saveFile1.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("FILETIME-" + "1");
        
        saveFile2.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("FILETIME-" + "2");

        saveFile3.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("FILETIME-" + "3");
    }
    public void PauseGame()
    {
        if(Time.timeScale == 1)
        {
            player.GetComponent<PlayerController>().isAttacking = true;
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            player.GetComponent<PlayerController>().isAttacking = false;
            Time.timeScale = 1;
            isPaused = false;
        }

        DOTween.Init();
        DOTween.defaultTimeScaleIndependent = true;
        DOTween.timeScale = 1;
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
    public void LoadSaveFile(Button  button)
    {

        if (button.gameObject.name == "SaveFile-1")
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
    public void enterHover(Button button)
    {
        if (delaySwitchScene == false)
        {
            uiSound.PlayOneShot(buttonHover);
            if (button.gameObject.name != "CreditsButton" && button.gameObject.name != "ControlsButton" && button.gameObject.name != "GraphicsButton" && button.gameObject.name != "AudioButton" && button.gameObject.name != "HeartIcon" && button.gameObject.name != "EggIcon" && button.gameObject.name != "SkullIcon")
            {
                button.gameObject.GetComponent<Image>().color = new Color32(120, 120, 120, 255);
            }
        }
    }
    public void exitHover(Button button)
    {
        if (button.gameObject.name != "CreditsButton" && button.gameObject.name != "ControlsButton" && button.gameObject.name != "GraphicsButton" && button.gameObject.name != "AudioButton" && button.gameObject.name != "HeartIcon" && button.gameObject.name != "EggIcon" && button.gameObject.name != "SkullIcon")
        {
            button.gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
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

    #region InputAction

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void FindInputActions()
    {
        inputActions.FindAction("Attack").performed += ctx => player.GetComponent<PlayerController>().ItemLMB();
        inputActions.FindAction("Escape").performed += ctx => EscapeInput();
        inputActions.FindAction("Inventory").performed += ctx => OpenInventory();
        inputActions.FindAction("Dash").performed += ctx => player.GetComponent<PlayerController>().Dash();
        inputActions.FindAction("Interact").performed += ctx => player.GetComponent<Dialog>().CheckClosestNPC();
        inputActions.FindAction("SelectSlot").performed += ctx => inventoryManager.SelectSlot();

        inputActions.FindAction("Move").performed += ctx => player.GetComponent<PlayerController>().Movement(ctx.ReadValue<Vector2>());
        inputActions.FindAction("Move").canceled += ctx => player.GetComponent<PlayerController>().Movement(ctx.ReadValue<Vector2>());
    }
    public void EscapeInput()
    {
        if (SceneManager.GetActiveScene().name == "InGame")
        {
            if (!inventoryMain.activeSelf && !dialogBox.activeSelf)
            {
                if (!isPaused)
                {
                    HideAllPanels();
                    PauseGame();
                    pauseMenu.SetActive(true);
                }
                else
                {
                    HideAllPanels();
                    PauseGame();
                }
            }
            else if (inventoryMain.activeSelf)
            {
                OpenInventory();
            }
            else if (dialogBox.activeSelf)
            {
                player.gameObject.GetComponent<Dialog>().CloseChat();
            }
        }
    }

    #endregion InputAction

    #region Menus/Buttons
    public void OpenInventory()
    {
        if (!inventoryMain.activeSelf)
        {
            inventoryMain.SetActive(true);
        }
        else
        {
            inventoryMain.SetActive(false);
        }
        PauseGame();
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

    public void HeartPopup(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("HeartPopup", button));
        }
        else if (sceneSwitch)
        {
            if(!healthPopup.activeSelf)
            {
                healthPopup.SetActive(true);
            }
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    public void EggsPopup(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("EggsPopup", button));
        }
        else if (sceneSwitch)
        {
            if (!eggsPopup.activeSelf)
            {
                eggsPopup.SetActive(true);
            }
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    public void RadiationPopup(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("RadiationPopup", button));
        }
        else if (sceneSwitch)
        {
            if (!radiationPopup.activeSelf)
            {
                radiationPopup.SetActive(true);
            }
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    public void HoverClosePopup(Button button)
    {

        sceneSwitch = false;
    }

    public void HoverOpenPopup(Button button)
    {

        sceneSwitch = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "HeartIcon" && healthPopup.activeSelf)
        {
            healthPopup.SetActive(true);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "SkullIcon" && radiationPopup.activeSelf)
        {
            radiationPopup.SetActive(true);
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "EggIcon" && eggsPopup.activeSelf)
        {
            eggsPopup.SetActive(true);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.gameObject.name == "HeartIcon" && healthPopup.activeSelf)
        {
            healthPopup.SetActive(false);
        }
        else if (eventData.selectedObject.gameObject.name == "SkullIcon" && radiationPopup.activeSelf)
        {
            radiationPopup.SetActive(false);
        }
        else if (eventData.selectedObject.gameObject.name == "EggIcon" && eggsPopup.activeSelf)
        {
            eggsPopup.SetActive(false);
        }
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
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                LoadFileNames();
            }
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
            PauseGame();
            pauseMenu.SetActive(false);
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    public void GraphicsButton(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("GraphicsButton", button));
        }
        else if (sceneSwitch)
        {
            graphicsPanel.SetActive(true);
            controlsPanel.SetActive(false);
            audioPanel.SetActive(false);
            graphicsButton.transform.localScale = new Vector3((float)1.2, (float)1.2, (float)1.2);
            audioButton.transform.localScale = new Vector3(1, 1, 1);
            controlsButton.transform.localScale = new Vector3(1, 1, 1);
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    public void AudioButton(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("AudioButton", button));
        }
        else if (sceneSwitch)
        {
            audioPanel.SetActive(true);
            controlsPanel.SetActive(false);
            graphicsPanel.SetActive(false);
            audioButton.transform.localScale = new Vector3((float)1.2, (float)1.2, (float)1.2);
            graphicsButton.transform.localScale = new Vector3(1, 1, 1);
            controlsButton.transform.localScale = new Vector3(1, 1, 1);
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    public void ControlsButton(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("ControlsButton", button));
        }
        else if (sceneSwitch)
        {
            controlsPanel.SetActive(true);
            audioPanel.SetActive(false);
            graphicsPanel.SetActive(false);
            controlsButton.transform.localScale = new Vector3((float)1.2, (float)1.2, (float)1.2);
            audioButton.transform.localScale = new Vector3(1, 1, 1);
            graphicsButton.transform.localScale = new Vector3(1, 1, 1);
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    public void BackButton()
    {
        //Returns to the Main Menu Screen
        SceneManager.LoadScene("MainMenu");
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
    #endregion Menus/Buttons

    
    
    #region LOADSAVE
    public void LOADFILE()
    {
        //savedTimeDate = GameObject.Find("savedTimeDate-" + file).GetComponent<TextMeshProUGUI>();
        //fileName = GameObject.Find("FileName-" + file).GetComponent<TMP_InputField>();

        //savedTimeDate.text = PlayerPrefs.GetString("FILETIME-" + file);
        //fileName.text = PlayerPrefs.GetString("FILENAME-" + file);

        //LOAD_PLAYER_HEALTH(file);
        //LOAD_PLAYER_LOCATION(file);

        if (!Application.isEditor)
        {
            LOAD_INVENTORY();
        }

    }
    void LOAD_PLAYER_HEALTH(int file)
    {
        //Sets the current Player Health, Health Text and Healthbar to the values from the selected Save File
        eggHealthRadiation.health = PlayerPrefs.GetFloat("PLAYER_HEALTH-" + file);
        eggHealthRadiation.damageDealt = PlayerPrefs.GetFloat("PLAYER_DAMAGEDEALT-" + file);
        eggHealthRadiation.healthMat.SetFloat("_RemovedSegments", PlayerPrefs.GetFloat("PLAYER_HEALTH_REMOVED_SEGMENTS-" + file));


        if(!PlayerPrefs.HasKey("PLAYER_HEALTH-" + file))
        {
            eggHealthRadiation.health = 50;
            eggHealthRadiation.damageDealt = 0;
        }
        eggHealthRadiation.UpdateHealth();
    }
    void LOAD_PLAYER_LOCATION(int file)
    {
        //Sets the current Player Position to the values from the selected Save File
        player.transform.position = new Vector2(PlayerPrefs.GetFloat("PLAYER_LOCATION_X-" + file), PlayerPrefs.GetFloat("PLAYER_LOCATION_Y-" + file));
    }
    public void LOAD_INVENTORY()
    {
        int file = PlayerPrefs.GetInt("CurrentFile");

        for (int i = 0; i < inventoryManager.inventorySlots.Length; i++)
        {
            if (PlayerPrefs.HasKey("INVENTORY-ITEM-NAME" + file + i))
            {
                InventorySlot slot = inventoryManager.inventorySlots[i];
                InventorySlot spawnSlot = inventoryManager.inventorySlots[PlayerPrefs.GetInt("INVENTORY-ITEM-SLOT" + file + i)];
                FindItemById(PlayerPrefs.GetString("INVENTORY-ITEM-NAME" + file + i), i.ToString(), file.ToString());

                inventoryManager.SpawnNewItem(currentItem, spawnSlot);

                slot.GetComponentInChildren<InventoryItem>().count = PlayerPrefs.GetInt("INVENTORY-ITEM-COUNT" + file + i);
                slot.GetComponentInChildren<InventoryItem>().countText.text = PlayerPrefs.GetInt("INVENTORY-ITEM-COUNT" + file + i).ToString();
            }
            else
            {
                continue;
            }
        }
    }
    
    public Item FindItemById(string name, string currentIndex, string currentFile)
    {
        for (int i = 0; i < items.Length; i++)
        {
            string itemName = name.Replace(currentFile + currentIndex, "");

            if (items[i].name == itemName)
            {
                currentItem = items[i];
            }
        }
        return null;
    }
    #endregion LOADSAVE

    public void SAVEFILE(int file)
    {
        DateTime currentDate = DateTime.Today;
        savedTimeDate = GameObject.Find("savedTimeDate-" + file).GetComponent<TextMeshProUGUI>();

        PlayerPrefs.SetString("FILETIME-" + file, currentDate.ToString("dd/MM/yyyy"));
        savedTimeDate.text = PlayerPrefs.GetString("FILETIME-" + file);

        //Saves the current Player Health and Position
        //SAVE_PLAYER_HEALTH(file);
        //SAVE_PLAYER_LOCATION(file);

        SAVE_INVENTORY();
    }
    #region SAVESAVE
    void SAVE_PLAYER_HEALTH(int file)
    {
        //Saves the current Player Health
        PlayerPrefs.SetFloat("PLAYER_HEALTH-" + file, eggHealthRadiation.health);
        PlayerPrefs.SetFloat("PLAYER_DAMAGEDEALT-" + file, eggHealthRadiation.damageDealt);
        PlayerPrefs.SetFloat("PLAYER_HEALTH_REMOVED_SEGMENTS-" + file, (int)eggHealthRadiation.removedSegments);
    }
    void SAVE_PLAYER_LOCATION(int file)
    {
        //Saves the current Player Position
        PlayerPrefs.SetFloat("PLAYER_LOCATION_X-" + file, player.transform.position.x);
        PlayerPrefs.SetFloat("PLAYER_LOCATION_Y-" + file, player.transform.position.y);
    }
    public void SAVE_INVENTORY()
    {
        int file = PlayerPrefs.GetInt("CurrentFile");

        for (int i = 0; i < inventoryManager.inventorySlots.Length; i++)
        {
            InventorySlot slot = inventoryManager.inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                PlayerPrefs.SetString("INVENTORY-ITEM-NAME" + file + i, itemInSlot.item.itemName);
                PlayerPrefs.SetInt("INVENTORY-ITEM-COUNT" + file + i, itemInSlot.count);
                PlayerPrefs.SetInt("INVENTORY-ITEM-SLOT" + file + i, i);

                Debug.Log(PlayerPrefs.GetInt("INVENTORY-ITEM-COUNT" + file + i));
                Debug.Log(PlayerPrefs.GetString("INVENTORY-ITEM-NAME" + file + i));
            }
            else
            {
                PlayerPrefs.DeleteKey("INVENTORY-ITEM-NAME" + file + i);
                PlayerPrefs.DeleteKey("INVENTORY-ITEM-COUNT" + file + i);
            }
            PlayerPrefs.Save();
        }
        


    }
    #endregion SAVESAVE

    
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

    private void LoadSettings()
    {
        LOAD_RESOLUTION();
        LOAD_FRAMERATE();
        LOAD_FULLSCREEN();
        LOAD_MUSIC_VOLUME();
        LOAD_SFX_VOLUME();
        if (sceneName == "InGame")
        {
            LOAD_FPS();
        }
    }
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