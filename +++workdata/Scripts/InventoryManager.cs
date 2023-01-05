using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public AudioSource sounds;
    public AudioClip pickup;

    public Item[] startItems;

    public int maxStackedItem = 12;
    public InventorySlot[] inventorySlots;

    public GameObject inventoryItemPrefab;

    int selectedSlot = -1;

    private void Start()
    {
        ChangeSelectedSlot(0);
        foreach(var item in startItems)
        {

        }
    }

    private void Update()
    {
        if(Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number > 0 && number < 6)
            {
                ChangeSelectedSlot(number -1);
            }
        }
    }
    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        
            inventorySlots[newValue].Select();
            selectedSlot = newValue;
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
                itemInSlot.RefreshCount(item);
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
                return true;
            }
        }
        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}