using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCdialog : MonoBehaviour
{
    private SpriteRenderer sr;

    public string npcName;

    public string[] dialog;

    private GameObject player;

    public Color32 npcColor;

    public int npcIndex = 0;

    public string[] dialogOrder;

    public Sprite npcSprite;

    public TextMeshProUGUI dialogText;
    public string[] randomSentencees;
    public GameObject dialogBox;



    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
    }

    void Start()
    {
        sr.sprite = npcSprite;
    }

    public void HideWorldSpaceDialog()
    {
        dialogBox.SetActive(false);
    }


    private void Update()
    {
        // Get the distance between the player and the object
        float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);

        // Check if the distance is less than 10 units
        if (distance > 1000000f)
        {
            // Generate a random number between 0 and 1
            float randomNumber = Random.Range(0f, 1f);

            // Check if the random number is less than 0.3
            if (randomNumber < 0.3f)
            {
                // Execute the first function
                dialogBox.SetActive(true);
            }
            else
            {
                // Execute the second function
                dialogBox.SetActive(false);
            }
        }
    }
}
