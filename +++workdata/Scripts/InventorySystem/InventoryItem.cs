using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Item item;
    public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    private Manager manager;

    InventoryManager inventoryManager;

    [Header("UI")]
    public Image image;
    public TextMeshProUGUI countText;
    private bool isDragging;

    private void Awake()
    {
        inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        RefreshCount();
    }

    private void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        RefreshCount();
        ScaleInventoryItem();
    }
    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.imageUI;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        countText.raycastTarget = false;
        parentAfterDrag = transform.parent;

        manager.originalSlot = transform.parent;
        transform.SetParent(transform.root);

        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        countText.raycastTarget = true;
        isDragging = false;
        ScaleInventoryItem();
        inventoryManager.CheckSelectedItem();
    }



    public void RefreshItemDescription()
    {
        TextMeshProUGUI text = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
        //text.text = item.itemName + " ";
        text.text = "";
        text.text += "</size>" + "\n" + "\n" + item.itemDescription;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RefreshItemDescription();

        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TextMeshProUGUI text = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
        text.text = "";
    }


    public void ScaleInventoryItem()
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
