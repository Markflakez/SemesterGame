using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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

    public void SelectSlot(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            ChangeSelectedSlot(1);
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
            inventoryItem.transform.GetChild(0).localScale = new Vector3(1, 1, 1);
        }
        else
        {
            inventoryItem.transform.GetChild(0).localScale = new Vector3((float)1.3, (float)1.3, (float)1.3);
        }
    }
}