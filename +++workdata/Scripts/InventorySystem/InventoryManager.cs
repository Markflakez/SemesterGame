using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

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

    public int selectedSlot = 0;

    private void Start()
    {
        ChangeSelectedSlot(selectedSlot);
        foreach (var item in startItems)
        {
        }
    }

    public void SelectSlot()
    {
        if(selectedSlot == 0)
        {
            ChangeSelectedSlot(1);
        }
        else
        {
            ChangeSelectedSlot(0);
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
            manager.escControl.color = manager.uiFontColorDisabled;
            manager.attackControl.color = Color.white;
            manager.interactControl.color = manager.uiFontColorDisabled;
            manager.useItemControl.color = Color.white;
            manager.dropItemControl.color = Color.white;
        }
        else if (selectedItemName == "Sword")
        {
            manager.escControl.color = manager.uiFontColorDisabled;
            manager.attackControl.color = Color.white;
            manager.interactControl.color = manager.uiFontColorDisabled;
            manager.useItemControl.color = manager.uiFontColorDisabled;
            manager.dropItemControl.color = Color.white;
        }
        else
        {
            manager.escControl.color = manager.uiFontColorDisabled;
            manager.attackControl.color = manager.uiFontColorDisabled;
            manager.interactControl.color = manager.uiFontColorDisabled;
            manager.useItemControl.color = manager.uiFontColorDisabled;
            manager.dropItemControl.color = manager.uiFontColorDisabled;
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
                CheckSelectedItem();
                Destroy(itemInSlot.gameObject);
            }
        }
        CheckSelectedItem();
    }


    public bool AddItem(Item item)
    {
        item.isDropped = false;

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItem && itemInSlot.item.stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
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
                return true;
            }
        }
        manager.inGameSound.PlayOneShot(manager.itemPickUp);
        CheckSelectedItem();
        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
        CheckSelectedItem();

    }
}