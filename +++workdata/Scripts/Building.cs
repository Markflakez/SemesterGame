using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cinemachine;

public class Building : MonoBehaviour
{
    private float switchDelay = .5f;

    private Manager manager;
    public bool hasEntered = false;

    public GameObject switchTo;

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
            manager.inputActions.FindAction("Move").Disable();

            if(switchTo.gameObject.name != "SStartHouseOutdoor-SPAWN")
            {
                manager.inGameSound.PlayOneShot(manager.openDoor);
            }
            yield return new WaitForSeconds(switchDelay);

            if (switchTo.gameObject.name == "StartHouseOutdoor-SPAWN")
            {
                manager.inGameSound.PlayOneShot(manager.closeDoor);
            }

            if (switchTo.gameObject.name == "eggShop-SPAWN" || switchTo.gameObject.name == "StartHouse-SPAWN")
            {
                manager.player.GetComponent<PlayerController>().idleState = 0;
            }
            else
            {
                manager.player.GetComponent<PlayerController>().idleState = 2;
            }
            manager.player.GetComponent<PlayerController>().anim.SetFloat("idleState", manager.player.GetComponent<PlayerController>().idleState);

            CinemachineVirtualCamera vCam = manager.playerCamera.GetComponent<CinemachineVirtualCamera>();

            manager.player.transform.position = switchTo.transform.position;
            vCam.gameObject.transform.position = switchTo.transform.position;

            vCam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
            vCam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
            vCam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
            yield return new WaitForSeconds(0.01f);
            manager.inputActions.FindAction("Move").Enable();
            manager.player.GetComponent<PlayerController>().CheckClosestNPC();
            manager.player.GetComponent<PlayerController>().CheckClosestBuilding();

            vCam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 1;
            vCam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 1;
            vCam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 1;
            hasEntered = false;
        }
    }
}
