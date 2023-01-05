using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadGame : MonoBehaviour
{
    //SaveFiles
    public GameObject saveFile1;
    public GameObject saveFile2;
    public GameObject saveFile3;


    private TextMeshProUGUI savedTimeDate;

    //Start is called before the first frame update
    void Start()
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


    public void LoadSaveFile(int file)
    {
        //The SaveFile selected by pressing one of the buttons is set as the active SaveFile, so that the selected SaveFile is loaded in the GameScene
        SceneManager.LoadScene("InGame");
        PlayerPrefs.SetInt("CurrentFile", file);
    }

    public void BackButton()
    {
        //Returns to the Main Menu Screen
        SceneManager.LoadScene("MainMenu");
    }
}
