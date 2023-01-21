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
    public ItemType type;

    [Header("UI")]
    public bool stackable = true;
    
    [Header("Both")]
    public Sprite imageIngame;
    public Sprite imageUI;


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
