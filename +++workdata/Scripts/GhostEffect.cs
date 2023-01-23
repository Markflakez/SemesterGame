using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    private PlayerController pc;


    private float invokeSpeed = 15f;



    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        pc = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        StartGhost();
        
    }

    private void StartGhost()
    {
        sr.sprite = pc.GetComponent<SpriteRenderer>().sprite;
        //if(pc.isDashing)
        {
            //Invoke("SpawnGhost", invokeSpeed);
        }
        //else
        {
           // Destroy(this.gameObject);
        }
    }
    private void SpawnGhost()
    {
        Instantiate(this).transform.position = pc.transform.position;
    }

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
        {
            Destroy(this.gameObject);
        }
    }
}
