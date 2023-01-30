using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class EnterBuilding : MonoBehaviour
{

    private Manager manager;

    public string buildingScene;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    public void Enter()
    {
        SceneManager.LoadScene(buildingScene, LoadSceneMode.Additive);
        SceneManager.MoveGameObjectToScene(manager.player, SceneManager.GetSceneByName(buildingScene));
        SceneManager.MoveGameObjectToScene(manager.playerCamera, SceneManager.GetSceneByName(buildingScene));
        SceneManager.MoveGameObjectToScene(manager.mainCanvas.gameObject, SceneManager.GetSceneByName(buildingScene));
        SceneManager.MoveGameObjectToScene(manager.mainCanvas.gameObject, SceneManager.GetSceneByName(buildingScene));
        SceneManager.MoveGameObjectToScene(manager.settingsMenu, SceneManager.GetSceneByName(buildingScene));
        SceneManager.MoveGameObjectToScene(manager.settingsMenu, SceneManager.GetSceneByName(buildingScene));
    }

}
