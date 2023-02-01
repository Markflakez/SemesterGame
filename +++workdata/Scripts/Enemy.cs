using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
using DG.Tweening;


public class Enemy : MonoBehaviour
{
    private GameObject player;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private EggHealthRadiation eggHealthRadiation;

    public int attackScheme = 0;

    public float minDistance;
    public float maxDistance;

    public Color hit;

    public float knockbackForce;
    public bool attackCooldown;
    public bool hasBeenAttacked;

    public float distance;
    public float attackRange;

    public float runSpeed;
    public float walKSpeed;
    public float runAwayDuration;

    private GameObject colliderPosObj;

    private GameObject middlepoint;

    private AIPath enemyAI;
    private AIDestinationSetter destinationSetter;

    private void Start()
    {
        enemyAI = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        middlepoint = new GameObject();
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        eggHealthRadiation = GameObject.Find("Manager").gameObject.GetComponent<Manager>().eggHealthRadiation;

        colliderPosObj = new GameObject();
        colliderPosObj.transform.parent = transform;
        colliderPosObj.transform.position = new Vector3(transform.position.x - 0.4f, transform.position.y + 1.4f, transform.position.z);

        destinationSetter.target = player.transform;
        hasBeenAttacked = false;
        attackCooldown = false;
    }

    private void Update()
    {
        //Calculates the distance between the Ghoul and the player
        distance = Vector2.Distance(gameObject.transform.position, player.transform.position);

        //If the player hasn't died yet
        if (!eggHealthRadiation.died)
        {
            //If the Ghoul is moving right and is not playing the attacking animation
            if (enemyAI.velocity.x > 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("GhoulAttack"))
            {
                //Flips the sprite to face right
                sr.flipX = true;
            }
            //If the Ghoul is moving left and is not playing the attacking animation
            else if (enemyAI.velocity.x < 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("GhoulAttack"))
            {
                //Flips the sprite to face left
                sr.flipX = false;
            }

            //If the Ghoul is within attack range and not on attack cooldown
            if (distance < attackRange && !attackCooldown)
            {
                //Starts the attack routine
                StartCoroutine(Attack());
            }

            //If the Ghoul has reached its destination and has not been attacked
            if (enemyAI.reachedDestination && !hasBeenAttacked)
            {
                //Starts the follow player function after a certain duration
                Invoke("FollowPlayer", runAwayDuration);
            }

            //If the Ghoul's velocity is not zero and it is not attacking or walking
            if (enemyAI.velocity != null && !anim.GetCurrentAnimatorStateInfo(0).IsName("GhoulAttack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("GhoulWalk"))
            {
                //Plays the walking animation
                anim.Play("GhoulWalk");
            }
        }

        else
        {
            //Plays the idle animation
            anim.Play("GhoulIdle");
            //Disables the scripts on the Ghoul
            gameObject.GetComponent<DistanceCheck>().DisableScripts();
        }
    }

    //This function is called when the Ghoul has been attacked
    public void HasBeenAttacked()
    {
        //Sets a random position for the Ghoul to run to
        RandomPos();
        //Sets the Ghoul's max speed to run speed
        enemyAI.maxSpeed = runSpeed;
        //Sets the hasBeenAttacked flag to true
        hasBeenAttacked = true;
        //Fades the color of the object from clear to purple over 3 seconds

        //After a certain duration, set hasBeenAttacked to false
        Invoke("hasBeenAttackedCoolDown", runAwayDuration);
    }

    // This function makes the Ghoul follow the player
    public void FollowPlayer()
    {
        //Sets the target of the destination setter to the player
        destinationSetter.target = player.transform;
        //Sets the Ghoul's max speed to walk speed
        enemyAI.maxSpeed = walKSpeed;
    }

    //Plays the attack animation and knocks the player away
    private IEnumerator Attack()
    {
        if(player.transform.position.x > gameObject.transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }



        attackCooldown = true;

        Vector2 knockbackDirection = (Vector2)transform.position - (Vector2)player.transform.position;
        knockbackDirection.Normalize();
        GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);


        anim.Play("GhoulAttack");
        eggHealthRadiation.Damage(25);
        while(enemyAI.reachedDestination)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        CanAttack();
    }

    private void CanAttack()
    {
        attackCooldown = false;
    }

    private void hasBeenAttackedCoolDown()
    {
        hasBeenAttacked = false;
    }


    //weniger gute umsetzung um den Gegner wilkürlich zu bewegen
    void RandomPos()
    {
        float randomDistance = Random.Range(minDistance, maxDistance);

        // Generate random angle
        float randomAngle = Random.Range(0f, 360f);

        // Calculate new position
        Vector3 newPosition = player.transform.position + Quaternion.Euler(0, 0, randomAngle) * Vector3.right * randomDistance;

        // Set the position of the objectToMove GameObject
        middlepoint.transform.position = newPosition;
        destinationSetter.target = middlepoint.transform;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(colliderPosObj.transform.position, attackRange);
    }
}