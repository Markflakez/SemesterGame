using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCdialog : MonoBehaviour
{
    private SpriteRenderer sr;

    public string npcName;

    public string[] dialog;


    public Color32 npcColor;

    public int npcIndex = 0;

    public string[] dialogOrder;

    public Sprite npcSprite;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        sr.sprite = npcSprite;
    }
}
