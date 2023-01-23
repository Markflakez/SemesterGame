using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f; // The speed at which the enemy moves
    public float attackRadius = 2f; // The distance at which the enemy will attack the player
    public float attackDelay = 1f; // The delay between attacks

    private Transform player; // Reference to the player's transform
    private bool attacking = false; // Whether or not the enemy is currently attacking
    private Rigidbody2D rb; // Reference to the enemy's Rigidbody2D component

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // Find the player's transform
        rb = GetComponent<Rigidbody2D>(); // Get the enemy's Rigidbody2D component
    }

    void Update()
    {
        // If the enemy is not currently attacking
        if (!attacking)
        {
            // Get the distance between the enemy and the player
            float distance = Vector2.Distance(transform.position, player.position);

            // If the distance is less than the attack radius
            if (distance < attackRadius)
            {
                // Start attacking
                StartCoroutine(Attack());
            }
            else
            {
                // Move towards the player
                rb.velocity = (player.position - transform.position).normalized * moveSpeed;

                // Rotate to face the player
                float angle = Mathf.Atan2(player.position.y - transform.position.y, player.position.x - transform.position.x) * Mathf.Rad2Deg;
                rb.rotation = angle;
            }
        }
    }

    IEnumerator Attack()
    {
        attacking = true;

        // Wait for attack delay
        yield return new WaitForSeconds(attackDelay);

        // Attack
        Debug.Log("Enemy attacked!");

        // Wait for another attack delay
        yield return new WaitForSeconds(attackDelay);

        attacking = false;
    }
}