using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]public Item item;
    public int count = 1;
    [HideInInspector]public Transform parentAfterDrag;


    [Header("UI")]
    public Image image;
    public Text countText;
    public TextMeshProUGUI infoText;
    public Image infoTextImage;
    private bool isDragging;

    private void Awake()
    {
        infoText.enabled = false;
        infoTextImage.enabled = false;
    }

    private void Start()
    {
        RefreshCount();
        ScaleInventoryItem();
    }
    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
        RefreshItemDescription();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);

        RefreshItemDescription();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);

        infoText.enabled = false;
        infoTextImage.enabled = false;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        isDragging = false;
        ScaleInventoryItem();
    }



    public void RefreshItemDescription()
    {
        infoText.text = "<size=60>" + item.itemName + " ";
        if(countText.text != "1")
        {
            infoText.text += "(" + countText.text + ")";
            infoText.text += "</size>" + "\n" + "\n" + item.itemDescription;
        }
        else
        {
            infoText.text += "</size>" + "\n" + "\n" + item.itemDescription;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDragging)
        {
            infoText.enabled = true;
            infoTextImage.enabled = true;
        }
        else
        {
            infoText.enabled = false;
            infoTextImage.enabled = false;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoText.enabled = false;
        infoTextImage.enabled = false;
    }


    void ScaleInventoryItem()
    {

        if (gameObject.transform.parent.transform.parent.name == "Inventory-Hotbar")
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (gameObject.transform.parent.transform.parent.name == "Inventory-Main")
        {
            gameObject.transform.localScale = new Vector3((float)1.3, (float)1.3, (float)1.3);
        }
    }
}
