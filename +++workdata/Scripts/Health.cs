using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int currentHealth;

    public int maxHealth;

    Manager manager;


    public Slider healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = currentHealth;

        manager = GameObject.Find("Manager").GetComponent<Manager>();

        healthBar.enabled = false;
    }

    public void TakeDamage(int damage)
    {
        healthBar.enabled = true;
        currentHealth -= damage;
        healthBar.value = currentHealth;

        if(currentHealth <= 0)
        {
            manager.inGameSound.PlayOneShot(manager.ghoulDeathSound);
            DestroyItself();
        }
        else if(currentHealth > 0)
        {
            manager.inGameSound.PlayOneShot(manager.ghoulHitSound);
        }
    }

    public void DestroyItself()
    {
        Destroy(gameObject);
    }

}
