using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class GhoulEnemy : MonoBehaviour
{
    // The UI element that will be used to display the health bar

    public float attackDistance;
    public float attackDelay;
    public int attackDamage;
    public float dashSpeed;
    public float dashDuration;

    private GameObject player;
    private float lastAttackTime;
    private bool dashing = false;
    private float dashStartTime;
    private bool knocked = false;
    public float followDistance;
    public float avoidDistance;

    public float randomMovementSpeed;
    public float randomMovementDelay;

    private float lastRandomMovementTime;
    private Vector3 randomMovementDirection;

    public float knockbackForce;

    private EggHealthRadiation eggHealthRadiation;
    public GameObject heart;
    public GameObject damageText;

    // The function that will be called to instantiate the Scriptable Objects
    public void HeartDrop()
    {
        // Create a random direction vector
        Vector3 randomDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

        // Instantiate the Scriptable Object in a random direction with a slight impulse
        GameObject spawnedObject = Instantiate(heart, transform.position, Quaternion.identity);
        spawnedObject.GetComponent<Rigidbody2D>().AddForce(randomDirection * 100);
    }



    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= attackDistance && !dashing && !knocked)
        {
            if (Time.time >= lastAttackTime + attackDelay)
            {
                StartCoroutine(Dash());
                lastAttackTime = Time.time;
            }
        }
        else if (Vector2.Distance(transform.position, player.transform.position) > followDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, dashSpeed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, player.transform.position) < avoidDistance)
        {
            AvoidOtherEnemies();
        }
        RandomMovement();
    }



    private void Start()
    {
        player = GameObject.Find("Player");
        eggHealthRadiation = GameObject.Find("Bars").GetComponent<EggHealthRadiation>();

        player = GameObject.FindWithTag("Player");
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the attack radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }


    IEnumerator Dash()
    {
        dashing = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = player.transform.position;
        float startTime = Time.time;
        float endTime = Time.time + dashDuration;
        while (Time.time < endTime)
        {
            float timeElapsed = Time.time - startTime;
            float percentageComplete = timeElapsed / dashDuration;
            float easeOutSine = Mathf.Sin(percentageComplete * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(startPos, endPos, easeOutSine);
            yield return null;
        }
        dashing = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Attack();
            knocked = true;
            Vector2 knockbackDirection = (transform.position - player.transform.position).normalized;
            collision.rigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
    void Attack()
    {
        eggHealthRadiation.Damage(attackDamage);
    }

    void AvoidOtherEnemies()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, avoidDistance);
        Vector2 avoidDirection = Vector2.zero;
        int enemiesCount = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].CompareTag("Ghoul") && enemies[i] != gameObject.GetComponent<Collider2D>())
            {
                enemiesCount++;
                avoidDirection += (Vector2)(transform.position - enemies[i].transform.position);
            }
        }
        if (enemiesCount > 0)
        {
            avoidDirection /= enemiesCount;
            avoidDirection = avoidDirection.normalized;
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)avoidDirection, dashSpeed * Time.deltaTime);
        }
    }

    void RandomMovement()
    {
        if (Time.time >= lastRandomMovementTime + randomMovementDelay)
        {
            lastRandomMovementTime = Time.time;
            randomMovementDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        }
        transform.position += randomMovementDirection * randomMovementSpeed * Time.deltaTime;
    }
}