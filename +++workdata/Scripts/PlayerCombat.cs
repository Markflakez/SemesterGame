using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using Pathfinding;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange;
    public int attackDamage;
    public float knockbackDistance;
    public float knockbackDuration;
    public float knockbackForce;

    public bool isAttacking = false;

    private Manager manager;

    [HideInInspector]
    [SerializeField]
    public GameObject colliderPosObj;

    public bool allowedToAttack = true;

    private Animator animator;
    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        manager = GameObject.Find("Manager").GetComponent<Manager>();


        colliderPosObj = new GameObject();
        colliderPosObj.transform.parent = transform;   
        colliderPosObj.transform.position = new Vector3(transform.position.x, transform.position.y + -1, transform.position.z);
    }


    public void Attack()
    {
        if (allowedToAttack)
        {
            manager.inGameSound.PlayOneShot(manager.swordSwingSound);
            if (playerController.idleState == 2)
            {
                animator.Play("PlayerAttackSwordDown");
            }
            else if (playerController.idleState == 0)
            {
                
                animator.Play("PlayerAttackSwordUp");
            }
            else if (playerController.idleState == 1)
            {
                
                animator.Play("PlayerAttackSwordRight");
            }
            else if (playerController.idleState == 3)
            {
                
                animator.Play("PlayerAttackSwordLeft");
            }


            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(colliderPosObj.transform.position, attackRange);

            // Loop through all colliders that were hit
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject.CompareTag("Enemy"))
                {
                    hitColliders[i].gameObject.GetComponent<Health>().TakeDamage(attackDamage);
                    hitColliders[i].gameObject.GetComponent<Enemy>().HasBeenAttacked();
                }
            }
            allowedToAttack = false;
            Invoke("AttackCooldown", (float).25);
        }
    }


    private void AttackCooldown()
    {
        allowedToAttack = true;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(colliderPosObj.transform.position, attackRange);
    }
}
