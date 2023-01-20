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
    public GameObject pc;
    public GameObject player;
    public GameObject manager;

    private GameObject currentNPC;

    public AudioClip typeSound;
    public AudioSource typeAudio;

    public InputAction interaction;
    public CinemachineVirtualCamera virtualCamera;

    public string playerName;
    private string npcName;

    public TextMeshProUGUI nameText;

    public Sprite gerhardtIcon;
    public Sprite richardtIcon;
    public Sprite rheinhardtIcon;
    public Sprite playerIcon;
    private Sprite npcIcon;
    public Image icon;


    public TextMeshProUGUI text;
    public TextMeshProUGUI e;

    private bool isInteracting;


    //Sätze in Arrays der jeweiligen NPCs
    private string[] sentences;
    public string[] sentencesGerhardt;
    public string[] sentencesRichardt; 
    public string[] sentencesRheinhardt;

    //Individuelle Farben für die NPCnamensTexte
    private Color32 nameTextColor;
    public Color32 colorGerhardt;
    public Color32 colorRichardt;
    public Color32 colorRheinhardt;

    private int index;
    public float typingSpeed;

    public bool canTalk = false;

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
    ////Wenn der Dialogtext der letzte Text aus dem Array ist, wird der NextButton deaktiviert.
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && isInteracting)
        {
            CloseChat();
        }
    }

    //Summary
    //Wenn der IEnumerator aufgerufen wird, wird ein Tippsound gespielt,
    //gecheckt ob der Text, welche dem jetztigen Index inspricht drei Leerzeichen enthält (wenn ja wird ihm die Eigenschaften des NPCs gegeben, wenn nicht des Spielers)
    //Jeder einzelne Buchstabe wird in das Dialogfeld mit dem Delay (TypingSpeed) hinzugefügt.
    //Wenn der Dialogtext dem Text aus dem jetzigen Index des Arrays entspricht, wird der NextButton aktiviert und der Tippsound gestoppt.
    IEnumerator Type()
    {

        typeAudio.PlayOneShot(typeSound);

        if (sentences[index].Contains("   "))
        {
            nameText.text = npcName;
            nameText.color = nameTextColor;
            icon.sprite = npcIcon;
        }
        else
        {
            nameText.text = playerName;
            nameText.color = new Color32(255, 0, 0, 255);
            icon.sprite = playerIcon;
        }

        foreach (char letter in sentences[index].ToCharArray())
        {
            text.text += letter;
            if (text.text == sentences[index])
            {
                typeAudio.Stop();
                if (index != sentences.Length -1)
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
        index = 0;
        text.text = "";
        manager.GetComponent<Manager>().inventoryHotbar.SetActive(true);
        manager.GetComponent<Manager>().inventoryHotbarBackBoard.SetActive(true);
        manager.GetComponent<Manager>().PauseGame();
        canTalk = true;
        typeAudio.Stop();
        text.enabled = false;
        continueButton.SetActive(false);
        dialogBox.SetActive(false);
        icon.enabled = false;
        pc.GetComponent<PlayerController>().canMove = true;
    }

    //Summary
    //Wenn der NextButton geklickt wird, wird die Funktion NextDialog aufgerufen.
    //Dabei wir der NextButton deaktiviert und wenn der Fortschritt im Array (Index) kleiner als die größe des Indexes ist der Index um eins addiert.
    //Falls Text im Dialogfeld ist wird er gelöscht und die Koroutine Type wird aufgerufen.
    public void NextDialog()
    {
        continueButton.SetActive(false);
        if (index < sentences.Length - 1)
        {
            index++;
            text.text = "";
            StartCoroutine(Type());
        }
    }

    //Summary
    //Wenn der Spieler einer der NPC Collision Felder betritt wird der jeweilige Array (der jeweiligen Sätze) verwiesen; inklusive der Farbe und des Namens des NPCs.
    //Zusätzlich wird das "E" Symbol, das dem Spieler vermittelt, dass er den NPC ansprechen kann, sichtbar gemacht.
    //Der canTalk bool ist dafür das der Dialog nicht sofort aufgerufen wird sondern der Spieler selbst ihn öffnen kann mittels der Interact Funktion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "NPC-Gerhardt")
        {
            sentences = sentencesGerhardt;
            nameTextColor = colorGerhardt;
            npcName = "Gerhardt";
            npcIcon = gerhardtIcon;

            e.enabled = true;
            canTalk = true;
        }
        else if (collision.gameObject.name == "NPC-Rheinhardt")
        {
            sentences = sentencesRheinhardt;
            nameTextColor = colorRheinhardt;
            npcName = "Rheinhardt";
            npcIcon = rheinhardtIcon;

            e.enabled = true;
            canTalk = true;
        }
        else if (collision.gameObject.name == "NPC-Richardt")
        {
            sentences = sentencesRichardt;
            nameTextColor = colorRichardt;
            npcName = "Richardt";
            npcIcon = richardtIcon;

            e.enabled = true;
            canTalk = true;
        }

    }

    //Summary
    //Wenn die Interact Funktion mittels des neuen Input Systems aufgerufen wird, wird der bool isInteracting aktiviert/deaktiviert.
    public void Interact(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            isInteracting = true;
        }
        if(context.canceled)
        {
            isInteracting = false;
        }
    }

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
    private void OnTriggerStay2D(Collider2D other)
    {
        if (canTalk && isInteracting && !virtualCamera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CameraPanOut"))
        {
            GameObject Gerhardt = GameObject.Find("NPC-Gerhardt");
            Gerhardt.GetComponent<DialogBox>().PlayerIsTalking();
            GameObject Rheinhardt = GameObject.Find("NPC-Rheinhardt");
            Rheinhardt.GetComponent<DialogBox>().PlayerIsTalking();
            GameObject Richardt = GameObject.Find("NPC-Richardt");
            Richardt.GetComponent<DialogBox>().PlayerIsTalking();


            manager.GetComponent<Manager>().inventoryHotbar.SetActive(false);
            manager.GetComponent<Manager>().inventoryHotbarBackBoard.SetActive(false);
            manager.GetComponent<Manager>().PauseGame();
            canTalk = false;
            e.enabled = false;
            text.enabled = true;
            dialogBox.SetActive(true);
            StartCoroutine(Type());
            icon.enabled = true;
            pc.GetComponent<PlayerController>().canMove = false;
        }
    }

    //Summary
    //Wenn der Spieler aus einem der NPC Collision Felder geht wird das E-Symbol deaktiviert.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "NPC-Rheinhardt" || collision.gameObject.name == "NPC-Richardt" || collision.gameObject.name == "NPC-Gerhardt")
        {
            e.enabled = false;
        }
    }
}
