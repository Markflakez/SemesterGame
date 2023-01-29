using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
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
    private TextMeshProUGUI infoText;

    private void Awake()
    {
        inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
    }

    private void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        infoText = manager.infoText;
        RefreshCount();
    }
    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
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
        inventoryManager.CheckSelectedItem();
    }



    public void RefreshItemDescription()
    {
        infoText.text = "";
        infoText.text += "</size>" + "\n" + "\n" + item.itemDescription;
        manager.itemNameHovered.text = item.itemName;
        manager.itemHovered.sprite = item.image;
        if (item.attackDamage != 0)
        {
            manager.itemAttackDamage.text = item.attackDamage.ToString();
            manager.itemAttackDamage.gameObject.transform.parent.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            manager.itemAttackDamage.text = "";
            manager.itemAttackDamage.gameObject.transform.parent.transform.parent.gameObject.SetActive(false);
        }
        if (item.healthBoost != 0)
        {
            manager.itemhealthBoost.text = item.healthBoost.ToString();
            manager.itemhealthBoost.gameObject.transform.parent.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            manager.itemhealthBoost.text = "";
            manager.itemhealthBoost.gameObject.transform.parent.transform.parent.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(manager.mainInventoryBG2.anchoredPosition.x == 270 && manager.inventoryMain.activeSelf)
        {
            manager.mainInventoryBG2.DOAnchorPosX(506, .5f).SetEase(Ease.InOutSine);
        }
        RefreshItemDescription();

        
    }

    public void nothin()
    {
        infoText.text = "";
        manager.itemAttackDamage.text = "";
        manager.itemhealthBoost.text = "";
        manager.itemHovered.sprite = manager.invisibleSprite;
        manager.itemNameHovered.text = "";
    }
}
