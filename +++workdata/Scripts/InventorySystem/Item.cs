using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{

    [Header("Gameplay")]

    public string itemName;
    public string itemDescription;
    public int itemCount;
    public int attackDamage;
    public int healthBoost;

    public bool isDropped = false;
    public ItemType type;

    [Header("UI")]
    public bool stackable = true;
    
    [Header("Both")]
    public Sprite image;


    public enum ItemType
    {
        Utilty,
        Tool
    }

    public enum ActionType
    {
        Dig, 
        Mine
    }
}
