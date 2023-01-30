using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cinemachine;

public class Building : MonoBehaviour
{

    private Manager manager;
    public bool hasEntered = false;

    public Vector3 spawnNextScene;

    public string buildingScene;
    public string currentScene;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }


    public IEnumerator Enter()
    {
        if (!hasEntered)
        {
            hasEntered = true;
            Debug.Log("YE");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildingScene.ToString(), LoadSceneMode.Additive);
            PlayerPrefs.SetFloat("PosY" + manager.file, spawnNextScene.y);
            PlayerPrefs.SetFloat("PosX" + manager.file, spawnNextScene.x);
            manager.sceneName = currentScene.ToString();
            while (!asyncLoad.isDone)
            {
                yield return null;
                manager.playerCamera.GetComponent<CinemachineVirtualCamera>().enabled = false;
            }

            SceneManager.MoveGameObjectToScene(manager.player, SceneManager.GetSceneByName(buildingScene));
            SceneManager.MoveGameObjectToScene(manager.playerCamera, SceneManager.GetSceneByName(buildingScene));
            SceneManager.MoveGameObjectToScene(manager.mainCanvas.gameObject, SceneManager.GetSceneByName(buildingScene));
            SceneManager.MoveGameObjectToScene(manager.gameSettings.gameObject, SceneManager.GetSceneByName(buildingScene));
            SceneManager.MoveGameObjectToScene(manager.gameObject, SceneManager.GetSceneByName(buildingScene));
            SceneManager.MoveGameObjectToScene(manager.postProcessingVolume.gameObject, SceneManager.GetSceneByName(buildingScene));
            SceneManager.MoveGameObjectToScene(manager.eventSystem.gameObject, SceneManager.GetSceneByName(buildingScene));
            manager.SpawnPos();
            manager.playerCamera.GetComponent<CinemachineVirtualCamera>().Follow = manager.player.transform;
            manager.playerCamera.GetComponent<CinemachineVirtualCamera>().enabled = true;

            SceneManager.UnloadSceneAsync(currentScene.ToString());
        }
    }
}
