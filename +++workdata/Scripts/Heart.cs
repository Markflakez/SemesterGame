using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Heart : MonoBehaviour
{
    private GameObject eggHealthRadiation;
    private GameObject player;

    public Transform target; // the object to follow
    public float acceleration = 60f; // the rate at which the object's speed increases
    public float maxSpeed = 30f; // the maximum speed of the object

    private float currentSpeed = 10f; // the current speed of the object




    private void Start()
    {
        eggHealthRadiation = GameObject.Find("Bars");
        player = GameObject.Find("Player");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            eggHealthRadiation.GetComponent<EggHealthRadiation>().addHealth(5);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 5)
        {

            // calculate the distance to the target
            Vector3 distance = player.transform.position - transform.position;

            // calculate the direction to the target
            Vector3 direction = distance.normalized;

            // update the object's speed
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

            // move the object in the direction of the target at the current speed
            transform.position += direction * currentSpeed * Time.deltaTime;
        }
    }

}
