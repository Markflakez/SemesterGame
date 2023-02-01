using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Dialog : MonoBehaviour
{
    public GameObject player;
    public GameObject manager;

    private GameObject middlePoint;
    private GameObject quarterPoint;

    private float time;

    private string[] currentDialog;

    public AudioClip typeSound;
    public AudioSource typeAudio;
    public CinemachineVirtualCamera virtualCamera;

    public string playerName;
    private string npcName;

    public TextMeshProUGUI nameText;

    public Sprite playerIcon;
    private int runThrough;

    public float range;
    public Vector2 checkPos;

    private bool freeEggs = true;


    public TextMeshProUGUI text;

    //Individuelle Farben für die NPCnamensTexte
    private Color32 nameTextColor;

    private int index;
    public float typingSpeed = 0.015f;

    public bool canTalk = true;

    //UI-Elemente für den Dialog
    public GameObject continueButton;
    public GameObject closeButton;
    public GameObject dialogBox;
    
    //Summary
    //Dialog-Elemente werden deaktiviert
    private void Awake()
    {
        if (manager.GetComponent<Manager>().sceneName == "InGame")
        {
            text.enabled = false;
            continueButton.SetActive(false);
            dialogBox.SetActive(false);
        }
    }

    IEnumerator Type()
    {
        GameObject closestNPC = manager.GetComponent<Manager>().closestNPC;

        CinemachineVirtualCamera vcam = manager.GetComponent<Manager>().playerCamera.GetComponent<CinemachineVirtualCamera>();

        vcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 3;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 3;

        if (closestNPC.GetComponent<NPCdialog>().dialogOrder[closestNPC.GetComponent<NPCdialog>().npcIndex].ToString() == "npc")
        {
            nameText.text = closestNPC.GetComponent<NPCdialog>().npcName;
            nameText.color = closestNPC.GetComponent<NPCdialog>().npcColor;

            if(closestNPC.GetComponent<NPCdialog>().npcName == "Laurel")
            {
                manager.GetComponent<Manager>().laurelChatAvatar.enabled = true;
                manager.GetComponent<Manager>().florusChatAvatar.enabled = false;
                manager.GetComponent<Manager>().pascalChatAvatar.enabled = false;
                manager.GetComponent<Manager>().morganChatAvatar.enabled = false;
            }
            else if (closestNPC.GetComponent<NPCdialog>().npcName == "Florus")
            {
                manager.GetComponent<Manager>().florusChatAvatar.enabled = true;
                manager.GetComponent<Manager>().laurelChatAvatar.enabled = false;
                manager.GetComponent<Manager>().pascalChatAvatar.enabled = false;
                manager.GetComponent<Manager>().morganChatAvatar.enabled = false;
            }
            else if (closestNPC.GetComponent<NPCdialog>().npcName == "Morgan")
            {
                manager.GetComponent<Manager>().morganChatAvatar.enabled = true;
                manager.GetComponent<Manager>().laurelChatAvatar.enabled = false;
                manager.GetComponent<Manager>().florusChatAvatar.enabled = false;
                manager.GetComponent<Manager>().pascalChatAvatar.enabled = false;
            }
            else if (closestNPC.GetComponent<NPCdialog>().npcName == "Pascal")
            {
                manager.GetComponent<Manager>().pascalChatAvatar.enabled = true;
                manager.GetComponent<Manager>().laurelChatAvatar.enabled = false;
                manager.GetComponent<Manager>().florusChatAvatar.enabled = false;
                manager.GetComponent<Manager>().morganChatAvatar.enabled = false;
            }
            manager.GetComponent<Manager>().playerChatAvatar.enabled = false;
            quarterPoint = new GameObject();
            quarterPoint.transform.position = (middlePoint.transform.position + closestNPC.transform.position) / 2;

            vcam.Follow = quarterPoint.transform;
        }
        else if (closestNPC.GetComponent<NPCdialog>().dialogOrder[closestNPC.GetComponent<NPCdialog>().npcIndex].ToString() == "player")
        {
            nameText.text = playerName;
            nameText.color = new Color32(255, 0, 0, 255);
            manager.GetComponent<Manager>().pascalChatAvatar.enabled = false;
            manager.GetComponent<Manager>().laurelChatAvatar.enabled = false;
            manager.GetComponent<Manager>().florusChatAvatar.enabled = false;
            manager.GetComponent<Manager>().morganChatAvatar.enabled = false;

            manager.GetComponent<Manager>().playerChatAvatar.enabled = true;

            quarterPoint.transform.position = (middlePoint.transform.position + player.transform.position) / 2;

            vcam.Follow = quarterPoint.transform;
        }

        manager.GetComponent<Manager>().inGameSound.PlayOneShot(typeSound);

        foreach (char letter in closestNPC.GetComponent<NPCdialog>().dialog[closestNPC.GetComponent<NPCdialog>().npcIndex].ToCharArray())
        {
            text.text += letter;
            if (text.text == closestNPC.GetComponent<NPCdialog>().dialog[closestNPC.GetComponent<NPCdialog>().npcIndex])
            {
                manager.GetComponent<Manager>().inGameSound.Stop();
                if (closestNPC.GetComponent<NPCdialog>().npcIndex != closestNPC.GetComponent<NPCdialog>().dialog.Length -1)
                {
                    continueButton.SetActive(true);
                }
            }

            yield return new WaitForSecondsRealtime(typingSpeed);
        }

    }

    //Summary
    //CloseChat wird aufgerufen, wenn der Spieler auf den CloseButton drückt.
    //Damit werden alle UI-Elemente, die zum Dialog gehören, geschlossen.
    //Der Text wird gelöscht und der index (der Fortschritt im Array) neugestartet.
    //Der Spieler kann sich wieder bewegen und die Kamera verfolgt wieder den Spieler.
    //Außerdem wird der instantiierte Mittelpunkt als Referenz für die Kamera gelöscht.
    public void CloseChat()
    {
        GameObject closestNPC = manager.GetComponent<Manager>().closestNPC;
        CinemachineVirtualCamera vcam = manager.GetComponent<Manager>().playerCamera.GetComponent<CinemachineVirtualCamera>();

        vcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 1;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 1;
        CinemachineZoomOut();
        Manager managerr = manager.GetComponent<Manager>();
        StopAllCoroutines();
        closestNPC.GetComponent<NPCdialog>().npcIndex = 0;
        runThrough = 0;
        text.text = "";
        managerr.inputActions.Enable();
        managerr.barIndicators.SetActive(true);
        managerr.questAvatar.SetActive(true);
        managerr.eggHealthRadiation.gameObject.SetActive(true);
        managerr.inventoryHotbar.SetActive(true);
        managerr.inventoryHotbarBackBoard.SetActive(true);
        managerr.itemList.SetActive(true);
        managerr.controlsPanel.SetActive(true);
        managerr.itemIndicators.SetActive(true);
        canTalk = true;
        manager.GetComponent<Manager>().inGameSound.Stop();
        text.enabled = false;
        continueButton.SetActive(false);
        dialogBox.SetActive(false);
        manager.GetComponent<Manager>().florusChatAvatar.enabled = false;
        manager.GetComponent<Manager>().laurelChatAvatar.enabled = false;
        manager.GetComponent<Manager>().pascalChatAvatar.enabled = false;
        manager.GetComponent<Manager>().morganChatAvatar.enabled = false;
        manager.GetComponent<Manager>().playerChatAvatar.enabled = false;
        player.GetComponent<PlayerController>().canMove = true;
        if(npcName == "Pascal" && freeEggs)
        {
            freeEggs = false;
            FreeEggs();
            manager.GetComponent<Manager>().inGameSound.PlayOneShot(manager.GetComponent<Manager>().itemEquip);
        }
    }

    //Summary
    //Wenn der NextButton geklickt wird, wird die Funktion NextDialog aufgerufen.
    //Dabei wir der NextButton deaktiviert und wenn der Fortschritt im Array (Index) kleiner als die größe des Indexes ist der Index um eins addiert.
    //Falls Text im Dialogfeld ist wird er gelöscht und die Koroutine Type wird aufgerufen.
    public void NextDialog()
    {
        GameObject closestNPC = manager.GetComponent<Manager>().closestNPC;
        continueButton.SetActive(false);
        if (closestNPC.GetComponent<NPCdialog>().npcIndex < closestNPC.GetComponent<NPCdialog>().dialog.Length - 1)
        {
            closestNPC.GetComponent<NPCdialog>().npcIndex ++;
            text.text = "";
            StartCoroutine(Type());
        }
    }


    public void FreeEggs()
    {
        for (int i = 0; i < 14; i++)
        {
            manager.GetComponent<Manager>().inventoryManager.AddItem(manager.GetComponent<Manager>().egg);
        }
    }

    //Summary
    //Wenn der Spieler einer der NPC Collision Felder betritt wird der jeweilige Array (der jeweiligen Sätze) verwiesen; inklusive der Farbe und des Namens des NPCs.
    //Zusätzlich wird das "E" Symbol, das dem Spieler vermittelt, dass er den NPC ansprechen kann, sichtbar gemacht.
    //Der canTalk bool ist dafür das der Dialog nicht sofort aufgerufen wird sondern der Spieler selbst ihn öffnen kann mittels der Interact Funktion
    //Summary
    //Wenn der Spieler in einem der NPC Kollisionsboxen steht,
    //er mit ihm reden kann und die Kamera Animation nicht in der Kamera Animation ist, welche gestartet wird,
    //wenn er aus einem Dialog rausgeht, werden die weitern Funktionen aufgerufen.
    //-Er kann nicht mehr den NPC Dialog öffnen
    //-Das E-Symbol ist nicht mehr aktiv
    //-Die UI-Elemente für den Dialog werden aktiviert
    //-Die Koroutine für den Ersten Index im Array wird gestartet
    //-Ein Mittelpunkt um die Kamera auf NPC und Spieler zu fokussieren wird erstellt und als ankerpunkt der Kamera gesetzt
    //-Eine Kamerafahrt(Zoom in die Mitte des NPCs und des Spielers) wird eingeleitet

    public void OpenChat()
    {
        Manager managerr = manager.GetComponent<Manager>();
        GameObject closestNPC = manager.GetComponent<Manager>().closestNPC;
        if (canTalk)
        {
            // Find all objects in the scene with the NPCScript script attached
            NPCdialog[] npcs = FindObjectsOfType<NPCdialog>();

            // Iterate through the list of NPCs
            foreach (NPCdialog npc in npcs)
            {
                // Execute the DoAction function on each NPC
                npc.HideWorldSpaceDialog();
            }


            CinemachineZoomIn();
            managerr.inputActions.Disable();
            managerr.itemIndicators.SetActive(false);
            managerr.barIndicators.SetActive(false);
            managerr.questAvatar.SetActive(false);
            managerr.eggHealthRadiation.gameObject.SetActive(false);
            managerr.inventoryHotbar.SetActive(false);
            managerr.inventoryHotbarBackBoard.SetActive(false);
            managerr.itemList.SetActive(false);
            canTalk = false;
            text.enabled = true;
            dialogBox.SetActive(true);

            player.GetComponent<PlayerController>().canMove = false;

            npcName = closestNPC.GetComponent<NPCdialog>().npcName;
            StartCoroutine(Type());
        }
    }

    //Zoom in the camera towards the closest NPC
    private void CinemachineZoomIn()
    {
        GameObject closestNPC = manager.GetComponent<Manager>().closestNPC;
        //Final size for the camera after the zoom
        float finalSize = 3;
        //Duration for the camera zoom
        float duration = 1;

        CinemachineVirtualCamera vcam = manager.GetComponent<Manager>().playerCamera.GetComponent<CinemachineVirtualCamera>();

        DOTween.To(() => vcam.m_Lens.OrthographicSize, x => vcam.m_Lens.OrthographicSize = x, finalSize, duration).SetTarget(vcam.m_Lens);


        middlePoint = new GameObject();
        //Sets the focus in the middle of player and closest NPC
        middlePoint.transform.position = (manager.GetComponent<Manager>().player.transform.position + closestNPC.transform.position) / 2;


        manager.GetComponent<Manager>().playerCamera.GetComponent<CinemachineVirtualCamera>().Follow = middlePoint.transform;
    }
    private void CinemachineZoomOut()
    {
        GameObject closestNPC = manager.GetComponent<Manager>().closestNPC;
        float finalSize = 5;
        float duration = (float)1.5;

        CinemachineVirtualCamera vcam = manager.GetComponent<Manager>().playerCamera.GetComponent<CinemachineVirtualCamera>();

        DOTween.To(() => vcam.m_Lens.OrthographicSize, x => vcam.m_Lens.OrthographicSize = x, finalSize, duration).SetTarget(vcam.m_Lens).SetEase(Ease.InOutSine).OnComplete(() => { player.GetComponent<PlayerController>().CheckClosestBuilding(); player.GetComponent<PlayerController>().CheckClosestBuilding(); });

        middlePoint.transform.position = (manager.GetComponent<Manager>().player.transform.position + closestNPC.transform.position) / 2;


        manager.GetComponent<Manager>().playerCamera.GetComponent<CinemachineVirtualCamera>().Follow = manager.GetComponent<Manager>().player.transform;
    }
}
