using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Sprite defaultSprite;
    public Sprite hoveredSprite;
    public Sprite pressedSprite;

    public Manager manager;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!manager.inventoryMain.activeSelf)
        {
            gameObject.GetComponent<Image>().sprite = hoveredSprite;
        }
        else if(manager.inventoryMain.activeSelf && gameObject.name == "SaveButton")
        {
            gameObject.GetComponent<Image>().sprite = hoveredSprite;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!manager.inventoryMain.activeSelf)
        {
            gameObject.GetComponent<Image>().sprite = defaultSprite;
        }
        else if (manager.inventoryMain.activeSelf && gameObject.name == "SaveButton")
        {
            gameObject.GetComponent<Image>().sprite = defaultSprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().sprite = pressedSprite;
    }
}