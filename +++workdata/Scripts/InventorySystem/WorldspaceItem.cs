using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldspaceItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D bc;

    public Item item;
    private Manager manager;

    private void Awake()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    private void Start()
    {
        sr.sprite = item.image;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            manager.inventoryManager.AddItem(item);
            Invoke("DestroyItself", .01f);
        }
        return;
    }
    private void DestroyItself()
    {
        Destroy(gameObject);
    }
}
