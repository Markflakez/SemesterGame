using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public AudioSource sounds;
    public AudioClip pickup;

    public Manager manager;
    public string selectedItemName;

    public Item[] startItems;

    public int maxStackedItem = 12;
    public InventorySlot[] inventorySlots;

    public GameObject inventoryItemPrefab;

    public int selectedSlot;

    private void Start()
    {
        ChangeSelectedSlot(0);
        foreach (var item in startItems)
        {
        }
    }

    public void SelectSlot_1(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            ChangeSelectedSlot(0);
        }
    }

    public void SelectSlot_2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ChangeSelectedSlot(1);
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        
        CheckSelectedItem();
    }

    public void CheckSelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            selectedItemName = itemInSlot.item.itemName;
        }
        else
        {
            selectedItemName = null;
        }

        if (selectedItemName == "Egg")
        {
            manager.eggIndicator.SetActive(true);
        }
        else
        {
            manager.eggIndicator.SetActive(false);
        }
    }

    public void RemoveItem(Item item)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            itemInSlot.count--;
            itemInSlot.RefreshCount();
            if(itemInSlot.count == 0)
            {
                Destroy(itemInSlot.gameObject);
            }
        }
    }


    public bool AddItem(Item item)
    {

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItem && itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                if(item.name == "Egg")
                {
                    sounds.PlayOneShot(pickup);
                }

                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                ScaleInventoryItem(slot);
                return true;
            }
        }
        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    void ScaleInventoryItem(InventorySlot inventoryItem)
    {

        if (inventoryItem.transform.parent.name == "Inventory-Hotbar")
        {
            inventoryItem.transform.GetChild(0).localScale = new Vector3((float).8, (float).8, (float).8);
        }
        else
        {
            inventoryItem.transform.GetChild(0).localScale = new Vector3((float)1.3, (float)1.3, (float)1.3);
        }
    }
}