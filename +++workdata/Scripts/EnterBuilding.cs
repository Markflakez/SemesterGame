using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class EnterBuilding : MonoBehaviour
{

    private Manager manager;

    public bool hasEntered = false;

    public string buildingScene;

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
            // The Application loads the Scene in the background at the same time as the current Scene.
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildingScene.ToString(), LoadSceneMode.Additive);

            // Wait until the last operation fully loads to return anything
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            SceneManager.LoadScene(buildingScene, LoadSceneMode.Additive);
            SceneManager.MoveGameObjectToScene(manager.player, SceneManager.GetSceneByName(buildingScene));
            SceneManager.MoveGameObjectToScene(manager.playerCamera, SceneManager.GetSceneByName(buildingScene));
            SceneManager.MoveGameObjectToScene(manager.mainCanvas.gameObject, SceneManager.GetSceneByName(buildingScene));
            SceneManager.MoveGameObjectToScene(manager.gameSettings.gameObject, SceneManager.GetSceneByName(buildingScene));
            SceneManager.MoveGameObjectToScene(manager.gameObject, SceneManager.GetSceneByName(buildingScene));
            SceneManager.MoveGameObjectToScene(manager.postProcessingVolume.gameObject, SceneManager.GetSceneByName(buildingScene));
            SceneManager.MoveGameObjectToScene(manager.eventSystem.gameObject, SceneManager.GetSceneByName(buildingScene));

            SceneManager.UnloadSceneAsync("InGame");
        }
    }

}
