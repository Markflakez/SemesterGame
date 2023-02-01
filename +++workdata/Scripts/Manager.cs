using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using UnityEngine.Video;
using UnityEngine.Rendering;

public class Manager : MonoBehaviour
{
    public AudioClip buttonSound;
    public AudioClip buttonHover;
    public AudioClip buttonClick;
    public AudioClip cough;
    public AudioClip piano;
    public AudioClip eggThrowSound;
    public AudioClip swordSwingSound;
    public AudioClip eggSplash;
    public AudioClip itemPickUp;
    public AudioClip dashSound;
    public AudioClip hurtSound1;
    public AudioClip hurtSound2;
    public AudioClip hurtSound3;
    public AudioClip ghoulHitSound;
    public AudioClip ghoulDeathSound;
    public AudioClip eggGroundHit;
    public AudioClip eggEnemyHit;
    public AudioClip openDoor;
    public AudioClip closeDoor;
    public AudioClip itemEquip;
    public AudioClip bushSound;
    public AudioClip eggMusic;
    public AudioClip ambientSounds;


    public AudioSource uiSound;
    public AudioSource inGameSound;
    public AudioSource ambientPlayer;

    private Button placeHolderButton;
    public Button saveButton;

    public Canvas mainCanvas;
    public Canvas optionsCanvas;

    public Image blackCanvas;
    public Image itemHovered;
    public Image playerChatAvatar;
    public Image laurelChatAvatar;
    public Image morganChatAvatar;
    public Image florusChatAvatar;
    public Image pascalChatAvatar;
    public RectTransform mainInventoryBG2;
    public RectTransform uiPanel;
    public Sprite invisibleSprite;
    public TextMeshProUGUI attackControl;
    public TextMeshProUGUI dropItemControl;
    public TextMeshProUGUI useItemControl;
    public TextMeshProUGUI escControl;
    public TextMeshProUGUI interactControl;
    public TextMeshProUGUI itemAttackDamage;
    public TextMeshProUGUI itemhealthBoost;
    public TextMeshProUGUI itemNameHovered;
    public TextMeshProUGUI questHeaderText;
    public TextMeshProUGUI questText;
    public TextMeshProUGUI savedTimeDate;
    public TextMeshProUGUI infoText; 
    public TextMeshProUGUI Task1;
    public TextMeshProUGUI Task2;
    public TextMeshProUGUI Task3;
    public TextMeshProUGUI ItemReward1;
    public TextMeshProUGUI ItemReward2;
    public TextMeshProUGUI ItemReward3;

    public Toggle fullscreen;
    public Toggle fps;
    public TMP_InputField fileName;

    public GameObject AvatarIcon;
    public GameObject blackCircle;
    public GameObject backDrop;
    public GameObject closestBuilding;
    public GameObject creditsButton;
    public GameObject creditsMenu;
    public GameObject dialogBox;
    public GameObject eggBack;
    public GameObject eggFront;
    public GameObject eggIndicator;
    public GameObject eggLeft;
    public GameObject eggPrefab;
    public GameObject eggRight;
    public GameObject eggShopSpawn;
    public GameObject inventoryHotbar;
    public GameObject inventoryHotbarBackBoard;
    public GameObject inventoryMain;
    public GameObject itemIndicators;
    public GameObject mainMenuBackBoard;
    public GameObject mainMenuQ;
    public GameObject pauseMenu;
    public GameObject player;
    public GameObject QuestPanel1;
    public GameObject QuestPanel2;
    public GameObject QuestPanel3;
    public GameObject questAvatar;
    public GameObject questLog;
    public GameObject quitMenu;
    public GameObject saveFile1;
    public GameObject saveFile2;
    public GameObject saveFile3;
    public GameObject saveFiles;
    public GameObject settingsMenu;
    public GameObject startHouseOutdoorSpawn;
    public GameObject startHouseSpawn;
    public GameObject swordPrefab;
    public GameObject PlayerBase;
    public GameObject graphicsPanel;
    public GameObject middleElements;
    public GameObject controlsPanel;
    public GameObject audioPanel;
    public GameObject chalkCheckmark;
    public GameObject candlesCheckmark;
    public GameObject goatSkullCheckmark;
    public GameObject goldenEggCheckmark;
    public GameObject itemList;
    public GameObject barIndicators;
    public GameObject playerCamera;
    public GameObject graphicsButton;
    public GameObject controlsButton;
    public GameObject audioButton;
    public GameObject healthPopup;
    public GameObject eggsPopup;
    public GameObject radiationPopup;
    public GameObject closestNPC;
    public GameObject TaskCheck1;
    public GameObject TaskCheck2;
    public GameObject TaskCheck3;
    public GameObject inputName; 
    public GameObject worldTime;
    public GameObject lightWhileBuilding;
    private GameObject spawnItem;

    public CinemachineBrain brain;
    public GameSettings gameSettings;
    public EggHealthRadiation eggHealthRadiation;
    public InputActionAsset inputActions;
    public Volume postProcessingVolume;
    public ChromaticAberration chromaticAberration;
    public ColorAdjustments colorAdjustments;
    public EventSystem eventSystem;
    public InventoryManager inventoryManager;
    public VideoPlayer videoPlayer;

    public Color radiationColor;
    public Color uiFontColor;
    public Color uiFontColorBrown;
    public Color uiFontColorDisabled;


    public Item egg;
    public Item sword;
    public Item[] items;
    public Item currentItem;



    public NPCdialog[] npcArray;
    public GameObject[] buildings;

    public int file;
    public int progress; 
    private Vector2 playerPos;
    [HideInInspector]
    public Transform originalSlot;

    public float closestDistance;
    public float buttonDelay = .15f;
    public float closestDistanceBuilding;
    public float closestDistanceNPC;
    public float distanceNPC = 300;

    public bool canEnter = true;
    public bool sceneSwitch = false;
    private bool delaySwitchScene = false;
    public bool saved = true;
    public bool backgroundburning = true;
    public bool isPaused = false;
    public bool canThrowEgg = true;

    public string sceneName;

    //Awake is called when the script instance is being loaded
    private void Awake()
    {
        //Gets the name of the current scene and store it in the sceneName variable
        sceneName = SceneManager.GetActiveScene().name;

        //If the scene name is not MainMenu or LoadGame
        if (sceneName != "MainMenu" && sceneName != "LoadGame")
        {
            //Gets the value of the "CurrentFile" key from the PlayerPrefs
            file = PlayerPrefs.GetInt("CurrentFile");
        }

        //Initializes the DOTween library
        DOTween.Init();
        //Sets DOTween's default time scale to be independent of Time.timeScale
        DOTween.defaultTimeScaleIndependent = true;

        if (sceneName != "LoadGame")
        {
            gameSettings = GameObject.Find("Settings").GetComponent<GameSettings>();
        }
    }

    //Start is called when the script is enabled and first frame has begun
    private void Start()
    {
        //If the application is being run in the Unity Editor
        if (Application.isEditor)
        {
            //Sets the "CurrentFile" key in PlayerPrefs to for easy playtesting in editor
            PlayerPrefs.SetInt("CurrentFile", 1);
        }

        //Calls a method that checks if the current file has saved data and if so it loads it
        CheckIfNewGame();

        //If the scene name is LoadGame
        if (sceneName == "LoadGame")
        {
            //Changes the buttons text to the string of the saveStates dates 
            LoadFileNames();
        }

        //If the scene name is not LoadGame
        if (sceneName != "LoadGame")
        {
            //Loads the graphic and volume settings
            LoadSettings();
        }
    }


    //If a certain PlayerPrefs Key is there, all Playerprefs will be loaded, if not the scene is prepared to play the intro sequence
    private void CheckIfNewGame()
    {
        if (sceneName == "InGame")
        {
            if (PlayerPrefs.HasKey("PLAYER_LOCATION_X-" + file))
            {
                LoadGame();
                StartBarCoroutines();
            }
            else
            {
                playerCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0, 0, 200);
                FindInputActions();
                NewGame();
            }
        }
    }

    //Initializes a new SaveStateFile and starts the intro sequence
    public void NewGame()
    {
        DeleteSaveFile(file);
        UpdateUIAlpha(0);

        blackCanvas.enabled = true;
        placeHolderButton = new GameObject().AddComponent<Button>();
        placeHolderButton.name = "placeHolderButton";

        player.transform.position = startHouseSpawn.transform.position;
        playerCamera.transform.position = startHouseSpawn.transform.position;

        SAVEFILE(placeHolderButton);
        StartCoroutine(PlayIntroSequence());
        EnableDamping();
    }

    //Starts the radiation, hunger and regenerate health
    public void StartBarCoroutines()
    {
        eggHealthRadiation.StartCoroutine(eggHealthRadiation.RadiationOverTime2());
        eggHealthRadiation.StartCoroutine(eggHealthRadiation.HungerOverTime());
        eggHealthRadiation.StartCoroutine(eggHealthRadiation.RegerateHealth());
    }

    public void LoadGame()
    {
        StopAllCoroutines();
        CancelInvoke();
        UpdateUIAlpha(1);
        videoPlayer.gameObject.SetActive(false);
        inputName.SetActive(false);
        blackCanvas.enabled = false;
        middleElements.SetActive(true);
        LOADFILE();
        EnableDamping();
        FindInputActions();
    }
    

    //The playerCamera damping is set back to it's original 
    private void EnableDamping()
    {
        CinemachineVirtualCamera vCam = playerCamera.GetComponent<CinemachineVirtualCamera>();
        vCam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 1;
        vCam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 1;
        vCam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 1;
    }

    //The intro sequence is started and after eight seconds the player can enter his name in the inputfield
    public IEnumerator PlayIntroSequence()
    {
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.enabled = true;
        videoPlayer.Play();
        yield return new WaitForSecondsRealtime(8);
        playerCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0, 0, -10);
        inputName.SetActive(true);
        inputName.GetComponentInChildren<TMP_InputField>().Select();
        videoPlayer.gameObject.transform.position = new Vector3(9999, 0, 0);
    }


    //If player has entered at least one letter and the inputfield is active, a black fadeout is started and the player can move
    public void OnEnterName(TMP_InputField inputField)
    {
        if (inputField.text != "" && inputName.activeSelf)
        {
            progress = 1;
            inputActions.FindAction("Enter").Disable();
            blackCanvas.gameObject.SetActive(true);
            inputName.SetActive(false);
            PlayerPrefs.SetString("PLAYER-NAME", inputField.text.ToString());
            player.GetComponent<Dialog>().playerName = PlayerPrefs.GetString("PLAYER-NAME");
            videoPlayer.gameObject.SetActive(false);
            float currentUIAlpha = 0;
            DOTween.To(() => currentUIAlpha, x => currentUIAlpha = x, 1, 1f).OnUpdate(() => UpdateUIAlpha(currentUIAlpha));
            FindInputActions();
            blackCanvas.DOColor(Color.clear, 3f).SetEase(Ease.Linear).OnComplete(() => blackCanvas.gameObject.SetActive(false));
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

    public void UpdateUIAlpha(float alpha)
    {
        uiPanel.GetComponent<CanvasGroup>().alpha = alpha;
    }

    private void HideAllPanels()
    {
        pauseMenu.SetActive(false);
        quitMenu.SetActive(false);
        mainMenuQ.SetActive(false);
        settingsMenu.SetActive(false);
    }
    public void LoadFileNames()
    {
        if (PlayerPrefs.HasKey("FILETIME-" + "1"))
        {
            saveFile1.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("FILETIME-" + "1");
            Debug.Log("yes");
        }
        else
        {
            saveFile1.GetComponentInChildren<TextMeshProUGUI>().text = "New Game";
        }
        if (PlayerPrefs.HasKey("FILETIME-" + "2"))
        {
            saveFile2.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("FILETIME-" + "2");
        }
        else
        {
            saveFile2.GetComponentInChildren<TextMeshProUGUI>().text = "New Game";
        }
        if (PlayerPrefs.HasKey("FILETIME-" + "3"))
        {
            saveFile3.GetComponentInChildren<TextMeshProUGUI>().text = PlayerPrefs.GetString("FILETIME-" + "3");
        }
        else
        {
            saveFile3.GetComponentInChildren<TextMeshProUGUI>().text = "New Game";
        }
    }
    public void PauseGame()
    {
        if(Time.timeScale == 1)
        {
            player.GetComponent<PlayerController>().isAttacking = true;
            inputActions.FindAction("Move").Disable();
            inputActions.FindAction("Attack").Disable();
            inputActions.FindAction("UseItem").Disable();
            inputActions.FindAction("DropItem").Disable();
            inputActions.FindAction("Dash").Disable();
            inputActions.FindAction("Interact").Disable();
            Time.timeScale = 0;
            StopCheckDistance();
            eggHealthRadiation.StopCoroutine(eggHealthRadiation.RadiationOverTime2());
            eggHealthRadiation.StopCoroutine(eggHealthRadiation.HungerOverTime());
            eggHealthRadiation.StopCoroutine(eggHealthRadiation.RegerateHealth());
            itemList.SetActive(false);
            itemIndicators.SetActive(false);
            isPaused = true;
        }
        else
        {
            player.GetComponent<PlayerController>().isAttacking = false;
            inputActions.FindAction("Move").Enable();
            inputActions.FindAction("Attack").Enable();
            inputActions.FindAction("UseItem").Enable();
            inputActions.FindAction("DropItem").Enable();
            inputActions.FindAction("Dash").Enable();
            inputActions.FindAction("Interact").Enable();
            Time.timeScale = 1;
            
            StartCheckDistance();
            eggHealthRadiation.StartCoroutine(eggHealthRadiation.RadiationOverTime2());
            eggHealthRadiation.StartCoroutine(eggHealthRadiation.HungerOverTime());
            eggHealthRadiation.StartCoroutine(eggHealthRadiation.RegerateHealth());
            itemList.SetActive(true);
            itemIndicators.SetActive(true);
            isPaused = false;
        }

        DOTween.Init();
        DOTween.timeScale = 1;
        playerCamera.GetComponent<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
        brain.m_IgnoreTimeScale = true;
    }


    public void StartCheckDistance()
    {
        DistanceCheck[] scriptObjects = GameObject.FindObjectsOfType<DistanceCheck>();

        foreach (DistanceCheck script in scriptObjects)
        {
            script.StartCheck();
        }
    }

    public void StopCheckDistance()
    {
        DistanceCheck[] scriptObjects = GameObject.FindObjectsOfType<DistanceCheck>();

        foreach (DistanceCheck script in scriptObjects)
        {
            script.StopCheck();
        }
    }

    //button sound is played with a random pitch to make it more versatile
    private void ButtonSound()
    {
        uiSound.PlayOneShot(buttonClick);
        uiSound.pitch = Random.Range(.8f, 1.2f);
    }

    //button is scaled down and back to it's original Size with a slight shake rotation
    private void ButtonAnimation(Button button)
    {
        Vector3 scale = button.gameObject.transform.localScale;
        Vector3 rotation = button.gameObject.transform.localEulerAngles;
        
        button.gameObject.transform.localScale = scale;
        button.gameObject.transform.localEulerAngles = rotation;

        button.gameObject.transform.DOScale(scale * 0.8f, buttonDelay * .5f).OnComplete(() =>
        {
        button.gameObject.transform.DOScale(scale * 1f, buttonDelay * .5f);
        });

        button.gameObject.transform.DOShakeRotation(buttonDelay, 10, 0, 50, true);
    }
    public void LoadSaveFile(Button button)
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
        savedTimeDate = button.gameObject.GetComponentInChildren<TextMeshProUGUI>();

        PlayerPrefs.SetString("FILETIME-" + file, currentDate.ToString("dd/MM/yyyy"));
        savedTimeDate.text = PlayerPrefs.GetString("FILETIME-" + file);


        if (!sceneSwitch)
        {
            PlayerPrefs.SetInt("CurrentFile", file);
            StartCoroutine(DelaySwitchScene("LoadSaveFile", button));

        }
        if (sceneSwitch)
        {

            SceneManager.LoadScene("InGame");
            
        }
        ButtonAnimation(button);
    }

    //all PlayerPrefs from current SaveState being deleted
    public void DeleteSaveFile(int file)
    {
        PlayerPrefs.DeleteKey("PLAYER_HEALTH-" + file);
        PlayerPrefs.DeleteKey("PLAYER_DAMAGEDEALT-" + file);
        PlayerPrefs.DeleteKey("PLAYER_HEALTH_REMOVED_SEGMENTS-" + file);
        PlayerPrefs.DeleteKey("PLAYER_LOCATION_X-" + file);
        PlayerPrefs.DeleteKey("PLAYER_LOCATION_Y-" + file);
        PlayerPrefs.DeleteKey("PLAYER_RADIATION-" + file);
        PlayerPrefs.DeleteKey("PLAYER_RADIATION_REMOVED_SEGMENTS-" + file);
        PlayerPrefs.DeleteKey("PLAYER_HUNGER-" + file);
        PlayerPrefs.DeleteKey("PLAYER_HUNGER_REMOVED_SEGMENTS-" + file);
        PlayerPrefs.DeleteKey("FILETIME-" + file);
        PlayerPrefs.DeleteKey("SAVE_SCENE" + file);
        PlayerPrefs.DeleteKey("Progress" + file);

        int i = 1;
        string[] keysDropped = { "droppedItem", "droppedItemPosX", "droppedItemPosY" };
        while (PlayerPrefs.HasKey("droppedItem" + i + file))
        {
            foreach (string key in keysDropped)
            {
                PlayerPrefs.DeleteKey(key + i + file);
            }
            i++;
        }

        string[] keysInventory = { "INVENTORY-ITEM-NAME", "INVENTORY-ITEM-COUNT", "INVENTORY-ITEM-SLOT" };
        for (int e = 0; e < 18; e++)
        {
            foreach (string key in keysInventory)
            {
                PlayerPrefs.DeleteKey(key + file + e);
            }
        }

        PlayerPrefs.Save();

        if (sceneName == "LoadGame")
        {
            LoadFileNames();
        }
    }
    

    IEnumerator DelaySwitchScene(string function, Button button)
    {
        if (delaySwitchScene == false)
        {
            delaySwitchScene = true;
            if (button.name != "placeHolderButton")
            {
                ButtonSound();
            }


            yield return new WaitForSecondsRealtime(buttonDelay);

            sceneSwitch = true;
            SendMessage(function, button);
            delaySwitchScene = false;

        }
    }

    #region InputAction

    private void FindInputActions()
    {
        

        if (progress == 0)
        {
            inputActions.FindAction("Enter").Enable();
            inputActions.FindAction("Enter").performed += ctx => OnEnterName(inputName.GetComponentInChildren<TMP_InputField>());
        }
        else
        {
            inputActions.Enable();
            inputActions.FindAction("Enter").Disable();
            inputActions.FindAction("Attack").performed += ctx => player.GetComponent<PlayerController>().ItemLMB();
            inputActions.FindAction("UseItem").performed += ctx => player.GetComponent<PlayerController>().UseItem();
            inputActions.FindAction("DropItem").performed += ctx => player.GetComponent<PlayerController>().DropItem();
            inputActions.FindAction("Dash").performed += ctx => player.GetComponent<PlayerController>().StartCoroutine(player.GetComponent<PlayerController>().Dash());
            inputActions.FindAction("Move").performed += ctx => player.GetComponent<PlayerController>().Movement(ctx.ReadValue<Vector2>());
            inputActions.FindAction("Move").canceled += ctx => player.GetComponent<PlayerController>().Movement(ctx.ReadValue<Vector2>());
            inputActions.FindAction("Interact").performed += ctx => Interact();
            inputActions.FindAction("Inventory").performed += ctx => OpenInventory();
            inputActions.FindAction("SelectSlot").performed += ctx => inventoryManager.SelectSlot();
            inputActions.FindAction("Escape").performed += ctx => EscapeInput();
        }
        
    }


    public void Interact()
    {
        if (closestDistanceBuilding < 2f && playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize == 5)
        {
            closestBuilding.GetComponent<Building>().StartCoroutine("Enter");
        }
        else if (closestNPC != null && closestDistanceNPC < 3 && playerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize == 5)
        {
            player.GetComponent<Dialog>().OpenChat();
        }
    }

    //Opens or closes Menu Panels in relation if their open
    public void EscapeInput()
    {
        //Checks if the current scene is not "LoadGame" or "MainMenu"
        if (SceneManager.GetActiveScene().name != "LoadGame" && SceneManager.GetActiveScene().name != "MainMenu")
        {
            //Checks if neither inventory or dialog box is active
            if (!inventoryMain.activeSelf && !dialogBox.activeSelf)
            {
                //Checks if the game is not paused
                if (!isPaused)
                {
                    //Hides all panels, pause the game, and activates the pause menu
                    HideAllPanels();
                    PauseGame();
                    pauseMenu.SetActive(true);
                }
                else
                {
                    //Hides all panels and pause the game
                    HideAllPanels();
                    PauseGame();
                }
            }
            //Checks if the inventory is active
            else if (inventoryMain.activeSelf)
            {
                //Opens the inventory
                OpenInventory();
            }
            //Checks if the dialog box is active
            else if (dialogBox.activeSelf)
            {
                //Closes the chat
                player.gameObject.GetComponent<Dialog>().CloseChat();
            }
        }
    }

    #endregion InputAction

    #region Menus/Buttons

    //Opens the Inventory
    public void OpenInventory()
    {
        if (!inventoryMain.activeSelf)
        {
            inventoryMain.SetActive(true);
        }
        else
        {
            mainInventoryBG2.localPosition = new Vector3(270, mainInventoryBG2.localPosition.y, mainInventoryBG2.localPosition.z);
            inventoryMain.SetActive(false);
        }
        PauseGame();
    }

    public void LoadGameScene(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("LoadGameScene", button));
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

    //Opens the PauseMenu and deactives other menus
    public void PauseMenu(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("PauseMenu", button));
        }
        else if (sceneSwitch)
        {
            pauseMenu.SetActive(true);
            settingsMenu.SetActive(false);
            mainMenuQ.SetActive(false);

            if (sceneName == "InGame")
            {
                quitMenu.SetActive(false);
            }

        }
        ButtonAnimation(button);
    }

    //Opens the SettingsMenu and deactives other menus
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
                graphicsPanel.SetActive(true);
                audioPanel.SetActive(false);
                controlsPanel.SetActive(false);
            }

            settingsMenu.SetActive(true);
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    //Opens the QuitMenu and deactives other menu
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

    //Opens the CreditsMenu and deactives other menus
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

    //Opens the Menu that questions if you really wanna quit without saving
    public void MainMenuQMenu(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("MainMenuQMenu", button));
        }
        else if (sceneSwitch)
        {
            SceneManager.LoadScene("MainMenu");
        }

        ButtonAnimation(button);
    }

    //Unpauses Game and deactivates all Menu Panels
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
            HideAllPanels();
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    //If a button is pressed and its gameobject name equals one of the three names, certain text and sprite is changed in the QuestMenu
    public void OpenQuest(Button button)
    {
        if(button.gameObject.name == "FirstQuest")
        {
            questText.text = "Embark on a quest to collect eggs from various locations across the land. Explore, solve puzzles, defeat enemies within a set time limit to complete the task. Receive a valuable reward for your efforts.";
            questAvatar.GetComponent<Image>().sprite = null;
            questHeaderText.text = button.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.ToString();
        }
        else if (button.gameObject.name == "SecondQuest")
        {
            questText.text = "avarage Bartholomäuschen";
            questAvatar.GetComponent<Image>().sprite = null;
            questHeaderText.text = button.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.ToString();
        }
        else if (button.gameObject.name == "ThirdQuest")
        {
            questText.text = "YEEEEEEEEEE";
            questAvatar.GetComponent<Image>().sprite = null;
            questHeaderText.text = button.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.ToString();
        }
    }

    //Opens up the GraphicsPanel and decreases Size of Controls and Audiopanel headers and deactive their relatives
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
            graphicsButton.transform.localScale = new Vector3((float)1.35, (float)1.35, (float)1.35);
            audioButton.transform.localScale = new Vector3(1, 1, 1);
            controlsButton.transform.localScale = new Vector3(1, 1, 1);
            controlsButton.GetComponentInChildren<TextMeshProUGUI>().color = uiFontColorDisabled;
            audioButton.GetComponentInChildren<TextMeshProUGUI>().color = uiFontColorDisabled;
            graphicsButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }

    //Opens up the AudioPanel and decreases Size of Controls and Graphicspanel headers and deactive their relatives
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
            audioButton.transform.localScale = new Vector3((float)1.35, (float)1.35, (float)1.35);
            graphicsButton.transform.localScale = new Vector3(1, 1, 1);
            controlsButton.transform.localScale = new Vector3(1, 1, 1);
            controlsButton.GetComponentInChildren<TextMeshProUGUI>().color = uiFontColorDisabled;
            audioButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            graphicsButton.GetComponentInChildren<TextMeshProUGUI>().color = uiFontColorDisabled;
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }
    
    //Opens up the ControlPanel and decreases Size of Graphics and Audiopanel headers and deactive their relatives
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
            controlsButton.transform.localScale = new Vector3((float)1.35, (float)1.35, (float)1.35);
            audioButton.transform.localScale = new Vector3(1, 1, 1);
            graphicsButton.transform.localScale = new Vector3(1, 1, 1);
            controlsButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            audioButton.GetComponentInChildren<TextMeshProUGUI>().color = uiFontColorDisabled;
            graphicsButton.GetComponentInChildren<TextMeshProUGUI>().color = uiFontColorDisabled;
            sceneSwitch = false;
        }

        ButtonAnimation(button);
    }
    //Returns you to the MainMenu
    public void MainMenuButton(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("MainMenuButton", button));
        }
        else if (sceneSwitch)
        {
            if(saved)
            {
                SceneManager.LoadScene("MainMenu");
                sceneSwitch = false;
            }
            else
            {
                HideAllPanels();
                mainMenuQ.SetActive(true);
                sceneSwitch = false;
            }
        }
        ButtonAnimation(button);
    }
    //Closes the application
    public void QuitGame(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("QuitGame", button));
            ButtonAnimation(button);
        }
        else if (sceneSwitch)
        {
            if (saved)
            {
                Application.Quit();
            }
            else
            {
                HideAllPanels();
                quitMenu.SetActive(true);
            }
            sceneSwitch = false;  
        }
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

        LOAD_INVENTORY();
        LOAD_WORLDSPACEITEMS();
        LOAD_PLAYER_LOCATION();
        LOAD_PLAYER_HEALTH();
        LOAD_PLAYER_RADIATION();
        LOAD_PLAYER_HUNGER();
        LOAD_TIME();
        LOAD_PROGRESS();
        player.GetComponent<Dialog>().playerName = PlayerPrefs.GetString("PLAYER-NAME");
        FindInputActions();
    }

    public void LOAD_WORLDSPACEITEMS()
    {
        int i = 1;
        int maxIterations = 17;
        while (i <= maxIterations)
        {
            string savedItem = "droppedItem" + i + file;
            string savedItemPosX = "droppedItem" + i + file + "PosX";
            string savedItemPosY = "droppedItem" + i + file + "PosY";
            if (PlayerPrefs.HasKey(savedItem))
            {
                SpawnItem(PlayerPrefs.GetString(savedItem), PlayerPrefs.GetFloat(savedItemPosX), PlayerPrefs.GetFloat(savedItemPosY));
                i++;
            }
            else
            {
                break;
            }
        }
    }

    private void DeleteOldPlayerPrefs()
    {
        int i = 1;
        while (PlayerPrefs.HasKey("droppedItem" + i + file))
        {
            PlayerPrefs.DeleteKey("droppedItem" + i + file);
            PlayerPrefs.DeleteKey("droppedItem" + i + file + "PosX");
            PlayerPrefs.DeleteKey("droppedItem" + i + file + "PosY");
            i++;
        }
    }

    public void SpawnItem(string savedItem, float posX, float posY)
    {
        spawnItem = new GameObject();
        spawnItem.transform.position = new Vector2(posX, posY);
        if (savedItem == "Egg")
        {
            Instantiate(eggPrefab, spawnItem.transform);
        }
        else if (savedItem == "Sword")
        {
            Instantiate(swordPrefab, spawnItem.transform);
        }
    }

    public void SAVE_WORLDSPACE_ITEMS()
    {
        DeleteOldPlayerPrefs();
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            WorldspaceItem inventoryItem = obj.GetComponent<WorldspaceItem>();
            if (inventoryItem != null && inventoryItem.item.isDropped)
            {
                SaveAndExecute(obj);
            }
        }
    }

    public void SaveAndExecute(GameObject obj)
    {
        int i = 1;
        while (true)
        {
            string savedItem = "droppedItem" + i + file;
            string savedItemPosX = "droppedItem" + i + file + "PosX";
            string savedItemPosY = "droppedItem" + i + file + "PosY";
            if (PlayerPrefs.HasKey(savedItem))
            {
                i++;
                continue;
            }
            PlayerPrefs.SetString(savedItem, obj.GetComponent<WorldspaceItem>().item.name);
            PlayerPrefs.SetFloat(savedItemPosX, obj.transform.position.x);
            PlayerPrefs.SetFloat(savedItemPosY, obj.transform.position.y);
            PlayerPrefs.Save();
            break;
        }
    }

    public void SAVE_TIME()
    {
        //PlayerPrefs.SetFloat("CURRENT-TIME" + file, worldTime.gameObject.GetComponent<DayNightCycle>().currentTime);
    }

    public void LOAD_TIME()
    {
        //worldTime.gameObject.GetComponent<DayNightCycle>().currentTime = PlayerPrefs.GetFloat("CURRENT-TIME" + file);
    }






    public void LOAD_PLAYER_HEALTH()
    {
        //Sets the current Player Health, Health Text and Healthbar to the values from the selected Save File
        eggHealthRadiation.health = PlayerPrefs.GetFloat("PLAYER_HEALTH-" + file);
        eggHealthRadiation.damageDealt = PlayerPrefs.GetFloat("PLAYER_DAMAGEDEALT-" + file);
        eggHealthRadiation.healthMat.SetInt("_RemovedSegments", PlayerPrefs.GetInt("PLAYER_HEALTH_REMOVED_SEGMENTS-" + file));

        eggHealthRadiation.UpdateHealth();
    }

    public void LOAD_PLAYER_HUNGER()
    {
        eggHealthRadiation.eggs = PlayerPrefs.GetFloat("PLAYER_HUNGER-" + file);
        eggHealthRadiation.eggMat.SetInt("_RemovedSegments", PlayerPrefs.GetInt("PLAYER_HUNGER_REMOVED_SEGMENTS-" + file));
        
        eggHealthRadiation.UpdateEggs();
    }

    public void LOAD_PLAYER_RADIATION()
    {
        eggHealthRadiation.speed = PlayerPrefs.GetFloat("PLAYER_RADIATION-" + file);
        eggHealthRadiation.radiationMat.SetInt("_RemovedSegments", PlayerPrefs.GetInt("PLAYER_RADIATION_REMOVED_SEGMENTS-" + file));
        eggHealthRadiation.MyFunction();
    }

    public void LOAD_PLAYER_LOCATION()
    {
        //Sets the current Player Position to the values from the selected Save File
        player.transform.position = new Vector2(PlayerPrefs.GetFloat("PLAYER_LOCATION_X-" + file), PlayerPrefs.GetFloat("PLAYER_LOCATION_Y-" + file));
        playerCamera.transform.position = new Vector2(PlayerPrefs.GetFloat("PLAYER_LOCATION_X-" + file), PlayerPrefs.GetFloat("PLAYER_LOCATION_Y-" + file));
    }
    public void LOAD_INVENTORY()
    {
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
    //Saves all Data from current SaveState in the PlayerPrefs
    public void SAVEFILE(Button button)
    {
        if (!sceneSwitch)
        {
            StartCoroutine(DelaySwitchScene("SAVEFILE", button));
            ButtonAnimation(button);
        }
        else if (sceneSwitch)
        {
            saved = true;
            if (button.name != "placeHolderButton")
            {
                button.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = uiFontColorDisabled;
            }

            DateTime currentDate = DateTime.Today;
            PlayerPrefs.SetString("FILETIME-" + file, currentDate.ToString("dd/MM/yyyy"));
            saveButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().gameObject.GetComponent<Animator>().Play("default");


            SAVE_INVENTORY();
            SAVE_WORLDSPACE_ITEMS();
            SAVE_PLAYER_LOCATION();
            SAVE_PLAYER_HEALTH();
            SAVE_PLAYER_RADIATION();
            SAVE_PLAYER_HUNGER();
            SAVE_TIME();
            SAVE_PROGRESS();
            sceneSwitch = false;
        }
    }
    #region SAVESAVE
    void SAVE_PLAYER_HEALTH()
    {
        //Saves the current Player Health
        PlayerPrefs.SetFloat("PLAYER_HEALTH-" + file, eggHealthRadiation.health);
        PlayerPrefs.SetFloat("PLAYER_DAMAGEDEALT-" + file, eggHealthRadiation.damageDealt);
        PlayerPrefs.SetInt("PLAYER_HEALTH_REMOVED_SEGMENTS-" + file, (int)eggHealthRadiation.removedSegments);
        PlayerPrefs.Save();
    }

    void SAVE_PLAYER_RADIATION()
    {
        //Saves the current Player Health
        PlayerPrefs.SetFloat("PLAYER_RADIATION-" + file, eggHealthRadiation.speed);
        PlayerPrefs.SetInt("PLAYER_RADIATION_REMOVED_SEGMENTS-" + file, (int)eggHealthRadiation.radiationRemovedSegments);
        PlayerPrefs.Save();
    }

    void SAVE_PLAYER_HUNGER()
    {
        //Saves the current Player Hunger
        PlayerPrefs.SetFloat("PLAYER_HUNGER-" + file, eggHealthRadiation.eggs);
        PlayerPrefs.SetInt("PLAYER_HUNGER_REMOVED_SEGMENTS-" + file, (int)eggHealthRadiation.eggRemovedSegments);
        PlayerPrefs.Save();
    }

    void SAVE_PROGRESS()
    {
        if (progress > 0)
        {
            PlayerPrefs.SetInt("PROGRESS" + file, progress);
        }
    }

    void LOAD_PROGRESS()
    {
        progress = PlayerPrefs.GetInt("PROGRESS" + file);
        if(progress == 0)
        {
            progress = 1;
        }
        FindInputActions();
    }

    void SAVE_PLAYER_LOCATION()
    {
        //Saves the current Player Position
        PlayerPrefs.SetFloat("PLAYER_LOCATION_X-" + file, player.transform.position.x);
        PlayerPrefs.SetFloat("PLAYER_LOCATION_Y-" + file, player.transform.position.y);
        PlayerPrefs.Save();
    }
    public void SAVE_INVENTORY()
    {
        for (int i = 0; i < inventoryManager.inventorySlots.Length; i++)
        {
            InventorySlot slot = inventoryManager.inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                PlayerPrefs.SetString("INVENTORY-ITEM-NAME" + file + i, itemInSlot.item.itemName);
                PlayerPrefs.SetInt("INVENTORY-ITEM-COUNT" + file + i, itemInSlot.count);
                PlayerPrefs.SetInt("INVENTORY-ITEM-SLOT" + file + i, i);
                PlayerPrefs.Save();
            }
            else
            {
                PlayerPrefs.DeleteKey("INVENTORY-ITEM-NAME" + file + i);
                PlayerPrefs.DeleteKey("INVENTORY-ITEM-COUNT" + file + i);
                PlayerPrefs.Save();
            }
            
        }
        


    }
    #endregion SAVESAVE

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
    //Loads all the graphic and audio settings
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

    //Resets all the graphic and audio settingso their default value
    public void ResetSettings()
    {
        RESET_FPS();
        RESET_FRAMERATE();
        RESET_FULLSCREEN();
        RESET_MUSIC_VOLUME();
        RESET_SFX_VOLUME();
        RESET_RESOLUTION();
    }

}