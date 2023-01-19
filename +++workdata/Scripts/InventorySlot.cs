using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;

    private Manager manager;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        Deselect();
    }


    public void Select()
    {
        image.color = selectedColor;
    }


    public void Deselect()
    {
        image.color = notSelectedColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        InventoryItem inventoryItem2 = gameObject.transform.GetComponentInChildren<InventoryItem>();

        if (transform.childCount == 0)
        {
            inventoryItem.parentAfterDrag = transform;
        }
        else if(transform.childCount > 0)
        {
            inventoryItem.parentAfterDrag = transform;
            inventoryItem2.transform.parent = manager.originalSlot;
        }
        inventoryManager.CheckSelectedItem();

        //inventoryItem.ScaleInventoryItem();
        //inventoryItem2.ScaleInventoryItem();
    }
}
