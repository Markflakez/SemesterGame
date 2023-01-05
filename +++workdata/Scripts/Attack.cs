using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public PlayerController pc;

    [HideInInspector]
    private Transform target;
    private int direction;

    private int bumpTime = 1;
    private int bumpForce = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Contains("NPC"))
        {
            Debug.Log("ICH BIN REICH");
            target = collision.transform;
            collision.gameObject.GetComponent<Animator>().Play("NPCHit");
            getDirection();
            //pc.weaponCollision.gameObject.SetActive(false);

        }
    }

    private void getDirection()
    {
        if (pc.idleState == 0)
        {
            direction = 0;
        }
        else if (pc.idleState == 1)
        {
            direction = 1;
        }
        else if (pc.idleState == 2)
        {
            direction = 2;
        }
        else if (pc.idleState == 3)
        {
            direction = 3;
        }
    }

    private void Update()
    {
        if (direction == 0)
        {
            target.position = Vector3.Lerp(target.position, new Vector3(target.position.x, target.position.y +bumpForce, target.position.z), Time.deltaTime * bumpTime);
        }
        else if (direction == 1)
        {
            target.position = Vector3.Lerp(target.position, new Vector3(target.position.x +bumpForce, target.position.y, target.position.z), Time.deltaTime * bumpTime);
        }
        else if (direction == 2)
        {
            target.position = Vector3.Lerp(target.position, new Vector3(target.position.x, target.position.y -bumpForce, target.position.z), Time.deltaTime * bumpTime);
        }
        else if (direction == 3)
        {
            target.position = Vector3.Lerp(target.position, new Vector3(target.position.x -bumpForce, target.position.y, target.position.z), Time.deltaTime * bumpTime);
        }
    }
}
