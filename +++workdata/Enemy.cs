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
        eggHealthRadiation = GameObject.Find("Bars").GetComponent<EggHealthRadiation>();

        colliderPosObj = new GameObject();
        colliderPosObj.transform.parent = transform;
        colliderPosObj.transform.position = new Vector3(transform.position.x - 0.4f, transform.position.y + 1.4f, transform.position.z);

        destinationSetter.target = player.transform;
        hasBeenAttacked = false;
        attackCooldown = false;
    }

    private void Update()
    {
        distance = Vector2.Distance(gameObject.transform.position, player.transform.position);


        if (enemyAI.velocity.x > 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("GhoulAttack"))
        {
            sr.flipX = true;
        }
        else if(enemyAI.velocity.x < 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("GhoulAttack"))
        {
            sr.flipX = false;
        }

        if(distance < attackRange && !attackCooldown)
        {
            Attack();
        }

        if(enemyAI.reachedDestination && !hasBeenAttacked)
        {
            Invoke("FollowPlayer", runAwayDuration);
        }


        if (enemyAI.velocity != null && !anim.GetCurrentAnimatorStateInfo(0).IsName("GhoulAttack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("GhoulWalk"))
        {
            anim.Play("GhoulWalk");
        }
    }

    public void HasBeenAttacked()
    {
        RandomPos();
        enemyAI.maxSpeed = runSpeed;
        hasBeenAttacked = true;
        // Fade the color of the object from clear to purple over 3 seconds

        Invoke("hasBeenAttackedCoolDown", runAwayDuration);
    }


    public void FollowPlayer()
    {
        destinationSetter.target = player.transform;
        enemyAI.maxSpeed = walKSpeed;
    }

    private void Attack()
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
        Invoke("CanAttack", 2);
    }

    private void CanAttack()
    {
        attackCooldown = false;
    }

    private void hasBeenAttackedCoolDown()
    {
        hasBeenAttacked = false;
    }

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