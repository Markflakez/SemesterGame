using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Dialog : MonoBehaviour
{
    public GameObject player;
    public GameObject manager;

    private string[] currentDialog;

    public AudioClip typeSound;
    public AudioSource typeAudio;
    public CinemachineVirtualCamera virtualCamera;

    public string playerName;
    private string npcName;

    public TextMeshProUGUI nameText;

    public Sprite playerIcon;
    private Sprite npcIcon;
    public Image icon;

    private float closestDistance = Mathf.Infinity;
    private int runThrough;

    public GameObject[] npcArray;
    public float range;
    public Vector2 checkPos;

    private GameObject closestObject;


    public TextMeshProUGUI text;

    //Individuelle Farben für die NPCnamensTexte
    private Color32 nameTextColor;

    private int index;
    public float typingSpeed;

    public bool canTalk = true;

    //UI-Elemente für den Dialog
    public GameObject continueButton;
    public GameObject closeButton;
    public GameObject dialogBox;
    
    //Summary
    //Dialog-Elemente werden deaktiviert
    private void Awake()
    {
        text.enabled = false;
        continueButton.SetActive(false);
        dialogBox.SetActive(false);
    }


    //Summary
    //Wenn der IEnumerator aufgerufen wird, wird ein Tippsound gespielt,
    //gecheckt ob der Text, welche dem jetztigen Index inspricht drei Leerzeichen enthält (wenn ja wird ihm die Eigenschaften des NPCs gegeben, wenn nicht des Spielers)
    //Jeder einzelne Buchstabe wird in das Dialogfeld mit dem Delay (TypingSpeed) hinzugefügt.
    //Wenn der Dialogtext dem Text aus dem jetzigen Index des Arrays entspricht, wird der NextButton aktiviert und der Tippsound gestoppt.
    IEnumerator Type()
    {

        if (closestObject.GetComponent<NPCdialog>().dialogOrder[closestObject.GetComponent<NPCdialog>().npcIndex].ToString() == "npc")
        {
            nameText.text = closestObject.GetComponent<NPCdialog>().npcName;
            nameText.color = closestObject.GetComponent<NPCdialog>().npcColor;
            icon.sprite = closestObject.GetComponent<NPCdialog>().npcSprite;
        }
        else if (closestObject.GetComponent<NPCdialog>().dialogOrder[closestObject.GetComponent<NPCdialog>().npcIndex].ToString() == "player")
        {
            nameText.text = playerName;
            nameText.color = new Color32(255, 0, 0, 255);
            icon.sprite = playerIcon;
        }

        typeAudio.PlayOneShot(typeSound);

        foreach (char letter in closestObject.GetComponent<NPCdialog>().dialog[closestObject.GetComponent<NPCdialog>().npcIndex].ToCharArray())
        {
            text.text += letter;
            if (text.text == closestObject.GetComponent<NPCdialog>().dialog[closestObject.GetComponent<NPCdialog>().npcIndex])
            {
                typeAudio.Stop();
                if (closestObject.GetComponent<NPCdialog>().npcIndex != closestObject.GetComponent<NPCdialog>().dialog.Length -1)
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
        StopAllCoroutines();
        closestObject.GetComponent<NPCdialog>().npcIndex = 0;
        runThrough = 0;
        text.text = "";
        manager.GetComponent<Manager>().inputActions.Enable();
        manager.GetComponent<Manager>().barIndicators.SetActive(true);
        manager.GetComponent<Manager>().questAvatar.SetActive(true);
        manager.GetComponent<Manager>().eggHealthRadiation.gameObject.SetActive(true);
        manager.GetComponent<Manager>().inventoryHotbar.SetActive(true);
        manager.GetComponent<Manager>().inventoryHotbarBackBoard.SetActive(true);
        manager.GetComponent<Manager>().PauseGame();
        canTalk = true;
        typeAudio.Stop();
        text.enabled = false;
        continueButton.SetActive(false);
        dialogBox.SetActive(false);
        icon.enabled = false;
        player.GetComponent<PlayerController>().canMove = true;
    }

    //Summary
    //Wenn der NextButton geklickt wird, wird die Funktion NextDialog aufgerufen.
    //Dabei wir der NextButton deaktiviert und wenn der Fortschritt im Array (Index) kleiner als die größe des Indexes ist der Index um eins addiert.
    //Falls Text im Dialogfeld ist wird er gelöscht und die Koroutine Type wird aufgerufen.
    public void NextDialog()
    {
        continueButton.SetActive(false);
        if (closestObject.GetComponent<NPCdialog>().npcIndex < closestObject.GetComponent<NPCdialog>().dialog.Length - 1)
        {
            closestObject.GetComponent<NPCdialog>().npcIndex ++;
            text.text = "";
            StartCoroutine(Type());
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
        if (canTalk)
        {
            manager.GetComponent<Manager>().inputActions.Disable();
            manager.GetComponent<Manager>().barIndicators.SetActive(false);
            manager.GetComponent<Manager>().questAvatar.SetActive(false);
            manager.GetComponent<Manager>().eggHealthRadiation.gameObject.SetActive(false);
            manager.GetComponent<Manager>().inventoryHotbar.SetActive(false);
            manager.GetComponent<Manager>().inventoryHotbarBackBoard.SetActive(false);
            manager.GetComponent<Manager>().PauseGame();
            canTalk = false;
            text.enabled = true;
            dialogBox.SetActive(true);

            icon.enabled = true;
            player.GetComponent<PlayerController>().canMove = false;

            npcIcon = closestObject.GetComponent<NPCdialog>().npcSprite;
            npcName = closestObject.GetComponent<NPCdialog>().npcName;
            StartCoroutine(Type());
        }
    }









    public void CheckClosestNPC()
    {
        closestObject = null;
        closestDistance = Mathf.Infinity;

        foreach (GameObject obj in npcArray)
        {
            float distance = Vector2.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestObject = obj;
                closestDistance = distance;
            }
        }

        if (closestDistance < 4f)
        {
            OpenChat();
        }

    }
}
