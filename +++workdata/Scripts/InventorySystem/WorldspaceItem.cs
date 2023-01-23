using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldspaceItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D bc;

    public Item item;
    private InventoryManager inventoryManager;

    private void Awake()
    {
        inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
    }

    private void Start()
    {
        sr.sprite = item.image;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            bool canAdd = inventoryManager.AddItem(item);
            if (canAdd)
            {
                Invoke("DestroyItself", .01f);
            }
        }
    }
    private void DestroyItself()
    {
        Destroy(gameObject);
    }
}
