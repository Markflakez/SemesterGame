using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange;
    public int attackDamage;
    public float knockbackForce;

    private Animator animator;
    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }


    public void Attack()
    {
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

        // Create an array to store all colliders that overlap the attack radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        // Loop through all colliders that were hit
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].GetComponent<Health>())
            {
                // Deal damage to the enemy's health
                hitColliders[i].GetComponent<Health>().TakeDamage(attackDamage);
                Vector2 forceDirection = hitColliders[i].GetComponent<Rigidbody2D>().transform.position - transform.position;
                hitColliders[i].GetComponent<Rigidbody2D>().AddForce(forceDirection.normalized * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        // Draw the attack radius in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
