using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/NPC")]
public class NPC : ScriptableObject
{

    [Header("Gameplay")]

    public string npcName;
    
    [Header("Both")]
    public Sprite image;

}
