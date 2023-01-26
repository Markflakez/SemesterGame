using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Health : MonoBehaviour
{
    public int currentHealth;

    public int maxHealth;
    public Color hit;
    Manager manager;


    public Slider healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = currentHealth;

        manager = GameObject.Find("Manager").GetComponent<Manager>();

        healthBar.gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        healthBar.gameObject.SetActive(true);
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


        gameObject.GetComponent<SpriteRenderer>().DOColor(Color.clear, .125f)
     // then fade it back to clear over 3 seconds
     .OnComplete(() => gameObject.GetComponent<SpriteRenderer>().DOColor(Color.white, .25f));

    }

    public void DestroyItself()
    {
        Destroy(gameObject);
    }

}
