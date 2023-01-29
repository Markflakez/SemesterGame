using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Manager manager;
    private void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.name == "FirstQuest" || gameObject.name == "SecondQuest" || gameObject.name == "ThirdQuest")
        {
            manager.uiSound.PlayOneShot(manager.buttonHover);
        }
        else
        {
            gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            manager.uiSound.PlayOneShot(manager.buttonHover);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (gameObject.name == "FirstQuest" || gameObject.name == "SecondQuest" || gameObject.name == "ThirdQuest")
        {
            manager.QuestPanel1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = manager.uiFontColorBrown;
            manager.QuestPanel1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = manager.uiFontColorBrown;

            manager.QuestPanel2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = manager.uiFontColorBrown;
            manager.QuestPanel2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = manager.uiFontColorBrown;

            manager.QuestPanel3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = manager.uiFontColorBrown;
            manager.QuestPanel3.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = manager.uiFontColorBrown;
        }

        if (gameObject.name == "FirstQuest")
        {
            gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        else if (gameObject.name == "SecondQuest")
        {
            gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        else if (gameObject.name == "ThirdQuest")
        {
            gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameObject.name == "FirstQuest" || gameObject.name == "SecondQuest" || gameObject.name == "ThirdQuest")
        {
            if (gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color != Color.white)
            {
                gameObject.GetComponentInChildren<TextMeshProUGUI>().color = manager.uiFontColorBrown;
            }
        }
        else
        {
            gameObject.GetComponentInChildren<TextMeshProUGUI>().color = manager.uiFontColorDisabled;
        }
    }


    private void Gray()
    {
        gameObject.GetComponentInChildren<TextMeshProUGUI>().color = manager.uiFontColorDisabled;
    }
}
