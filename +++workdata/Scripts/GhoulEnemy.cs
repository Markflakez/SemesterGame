using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class GhoulEnemy : MonoBehaviour
{
    // The maximum health of the character
    public int maxHealth = 100;

    // The current health of the character
    public int currentHealth = 100;

    // The UI element that will be used to display the health bar
    public Slider healthBar;

    Vector3 dis;

    private EggHealthRadiation eggHealthRadiation;

    public Dialog dialog;
    private GameObject player;
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public Animator anim;
    public float speed;

    private bool notPlaying;

    private float distance;

    public float currentSpeed;
    public float maxSpeed;
    public float accelerationSpeed;

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




    private void Update()
    {
        
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 5)
        {

            // calculate the distance to the target
            Vector3 distance = player.transform.position - transform.position;

            // calculate the direction to the target
            Vector3 direction = distance.normalized;

            // update the object's speed
            currentSpeed += accelerationSpeed * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

            // move the object in the direction of the target at the current speed
            transform.position += direction * currentSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            Damage();
            InvokeRepeating("Damage", 1, 1);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        CancelInvoke("Damage");
    }
    private void Damage()
    {

        eggHealthRadiation.Damage(15);

    }

    private void DamageTextSpawn(int damage)
    {
        // Create a random direction vector
        Vector3 randomDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

        // Instantiate the Scriptable Object in a random direction with a slight impulse
        GameObject spawnedObject = Instantiate(damageText, transform.position, Quaternion.identity);
        spawnedObject.GetComponent<TextMeshPro>().text += damage;
    }

    public void OnHit(int damage)
    {
        DamageTextSpawn(damage);

        float randomPercentage = Random.value;
        if(randomPercentage < .2)
        {
            HeartDrop();
            randomPercentage = Random.value;
            if (randomPercentage < .4)
            {
                HeartDrop();
                randomPercentage = Random.value;
                if(randomPercentage < .2)
                {
                    HeartDrop();
                }
            }
        }

        currentHealth -= damage;
        healthBar.value = currentHealth;
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        healthBar.value = maxHealth;

        eggHealthRadiation = GameObject.Find("Bars").GetComponent<EggHealthRadiation>();
        player = GameObject.Find("Player");
    }
}