using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonAnimation : MonoBehaviour
{
    public float scaleAmount = 1.2f;

    private Manager manager;


    private void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    public void OnPointerEnter(BaseEventData baseEventData)
    {
        // Scale the button down
        transform.DOScale(scaleAmount, 0.1f);
    }

    public void OnPointerExit(BaseEventData baseEventData)
    {
        // Scale the button back to its original size
        transform.DOScale(1f, 0.1f);
    }



    public void OnPointerClick(BaseEventData baseEventData)
    {
        Invoke("SwitchMenu", .3f);

    }
    
    public void SwitchMenu()
    {
        if (gameObject.name == "SaveGame-Button")
        {
            manager.SaveMenu();
        }
        else if (gameObject.name == "Settings-Button")
        {
            manager.SettingsMenu();
        }
        else if (gameObject.name == "MainMenu-Button")
        {
            manager.MainMenuButton();
        }
    }



}
