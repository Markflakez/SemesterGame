using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurningAnimation : MonoBehaviour
{

    public Image background;
    public Sprite backgroundDefault;
    public Sprite backgroundFire;

    private Manager manager;
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        if(manager.backgroundburning)
        {
            GetComponent<Image>().enabled = true;
            background.sprite = backgroundFire;
            GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            GetComponent<Image>().enabled = false;
            background.sprite = backgroundDefault;
            GetComponent<AudioSource>().enabled = false;
        }
    }
}
