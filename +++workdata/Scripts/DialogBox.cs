using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public string[] randomSentencees;
    private GameObject player;
    public GameObject dialogBox;

    private bool updateDialog = true;

    private string currentSentence;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }


    void Start()
    {
        
    }


    public void PlayerIsTalking()
    {
        updateDialog = true;
        dialogBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float dist = (Vector3.Distance(this.transform.position, player.transform.position));
        if(dist < 4 && updateDialog)
        {
            updateDialog = false;
            dialogText.text = randomSentencees[Random.Range(0, randomSentencees.Length)];
            dialogBox.SetActive(true);
        }
        if(dist > 6 && !updateDialog)
        {
            updateDialog = true;
            dialogBox.SetActive(false);
        }
    }
}
