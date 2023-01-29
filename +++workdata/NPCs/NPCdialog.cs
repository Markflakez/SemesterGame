using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class NPCdialog : MonoBehaviour
{
    private SpriteRenderer sr;

    public string npcName;

    public int randomInt;

    public string[] dialog;

    private GameObject player;

    public Color32 npcColor;

    public int npcIndex = 0;

    public string[] dialogOrder;

    public Sprite npcSprite;

    public Manager manager;

    public TextMeshProUGUI dialogText;
    public string[] randomSentencees;
    public GameObject dialogBox;

    private Color endColor = Color.white;

    private bool visibleBox = false;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    void Start()
    {
        sr.sprite = npcSprite;
    }

    public void HideWorldSpaceDialog()
    {
        dialogText.text = "";
        dialogBox.SetActive(false);
    }

    private void RandomSentence()
    {
        if (dialogText.text == "")
        {
            dialogText.text = "";
            dialogText.text = randomSentencees[Random.Range(0, randomSentencees.Length)];
        }
    }


    private void Update()
    {
        // Get the distance between the player and the object
        float distance = Vector3.Distance(player.transform.position, gameObject.transform.position);

        // Check if the distance is less than 10 units
        if (distance < 5f)
        {
            FadeInWorldspaceDialog();
        }
        else if(distance > 8f)
        {
            FadeOutWorldspaceDialog();
        }
        if(distance < 3f)
        {
            manager.interactControl.color = Color.white;
        }
        else
        {
            manager.interactControl.color = manager.uiFontColorDisabled;
        }
    }


    public void FadeOutWorldspaceDialog()
    {
        if (!DOTween.IsTweening(dialogBox) && visibleBox)
        {
            visibleBox = false;
            Vector2 normalScale = new Vector2(1, 1);
            dialogBox.transform.DOScale((normalScale * (float).8) * 1f, 1);
            DOTween.To(() => dialogBox.GetComponent<Image>().color, x => dialogBox.GetComponent<Image>().color = x, Color.clear, 1).OnComplete(() => HideWorldSpaceDialog());
            DOTween.To(() => dialogBox.GetComponentInChildren<TextMeshProUGUI>().color, x => dialogBox.GetComponentInChildren<TextMeshProUGUI>().color = x, Color.clear, 1);
        }
        return;
    }

    public void FadeInWorldspaceDialog()
    {
        if (!DOTween.IsTweening(dialogBox) && !visibleBox)
        {
            visibleBox = true;
            randomInt = Random.Range(1, 2);
            if (randomInt == 1)
            {
                randomInt = -1;
                dialogBox.transform.localScale = new Vector2((float).8, (float).8);
                dialogBox.SetActive(true);
                dialogBox.GetComponent<Image>().color = Color.clear;
                dialogBox.GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;

                RandomSentence();

                Vector2 normalScale = new Vector2(1, 1);

                dialogBox.transform.DOScale(normalScale * 1f, 1);
                DOTween.To(() => dialogBox.GetComponent<Image>().color, x => dialogBox.GetComponent<Image>().color = x, endColor, 1);
                DOTween.To(() => dialogBox.GetComponentInChildren<TextMeshProUGUI>().color, x => dialogBox.GetComponentInChildren<TextMeshProUGUI>().color = x, endColor, 1);
            }
        }
        return;
    }

}